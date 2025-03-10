using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using BinarySerialization.Constants;
using BinarySerialization.CustomEventArgs;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test
{
    public abstract class TestBase
    {
        protected static readonly BinarySerializer Serializer = new();

        protected static readonly BinarySerializer SerializerBe = new()
        {
            Endianness = Endianness.Big
        };

        protected static readonly string[] TestSequence = ["a", "b", "c"];
        protected static readonly int[] PrimitiveTestSequence = [1, 2, 3];

        protected TestBase(bool registerEvent = true)
        {
            if (registerEvent)
            {
                Serializer.MemberSerializing += OnMemberSerializing;
                Serializer.MemberSerialized += OnMemberSerialized;
                Serializer.MemberDeserializing += OnMemberDeserializing;
                Serializer.MemberDeserialized += OnMemberDeserialized;
            }
        }

        public static T Roundtrip<T>(T o, object context = null)
        {
            PrintSerialize(typeof(T));

            MemoryStream stream = new();
            Serialize(stream, o, context);

            stream.Position = 0;

            PrintDeserialize(typeof(T));
            return Deserialize<T>(stream, context);
        }

        protected static T Roundtrip<T>(T o, long expectedLength)
        {
            PrintSerialize(typeof(T));
            MemoryStream stream = new();
            Serialize(stream, o);

            stream.Position = 0;
            byte[] data = stream.ToArray();

            Assert.AreEqual(expectedLength, data.Length);

            PrintDeserialize(typeof(T));
            return Deserialize<T>(stream);
        }


        protected static T Roundtrip<T>(T o, byte[] expectedValue)
        {
            PrintSerialize(typeof(T));
            MemoryStream stream = new();
            Serialize(stream, o);

            stream.Position = 0;
            byte[] data = stream.ToArray();

            AssertEqual(expectedValue, data);

            PrintDeserialize(typeof(T));
            return Deserialize<T>(stream);
        }

        protected static T RoundtripBigEndian<T>(T o, long expectedLength)
        {
            PrintSerialize(typeof(T));
            MemoryStream stream = new();
            SerializeBe(stream, o);

            stream.Position = 0;
            byte[] data = stream.ToArray();

            Assert.AreEqual(expectedLength, data.Length);

            PrintDeserialize(typeof(T));
            return DeserializeBe<T>(stream);
        }

        protected static T RoundtripBigEndian<T>(T o, byte[] expectedValue)
        {
            PrintSerialize(typeof(T));
            MemoryStream stream = new();
            SerializeBe(stream, o);

            stream.Position = 0;
            byte[] data = stream.ToArray();

            AssertEqual(expectedValue, data);

            PrintDeserialize(typeof(T));
            return DeserializeBe<T>(stream);
        }

        private static void AssertEqual(byte[] expected, byte[] actual)
        {
            int length = Math.Min(expected.Length, actual.Length);

            for (int i = 0; i < length; i++)
            {
                byte e = expected[i];
                byte a = actual[i];

                Assert.AreEqual(a, e, $"Value at position {i} does not match expected value.  Expected 0x{e:X2}, got 0x{a:X2}");
            }

            Assert.AreEqual(actual.Length, expected.Length, $"Sequence lengths do not match.  Expected {expected.Length}, got {actual.Length}");
        }

        protected static T RoundtripReverse<T>(byte[] data)
        {
            T o = Deserialize<T>(data);

            return Roundtrip(o, data);
        }

        protected static T Deserialize<T>(string filename)
        {
            using FileStream stream = new(filename, FileMode.Open, FileAccess.Read);
            PrintDeserialize(typeof(T));
            return Deserialize<T>(stream);
        }

        protected static T Deserialize<T>(byte[] data)
        {
            PrintDeserialize(typeof(T));
            return Deserialize<T>(new MemoryStream(data));
        }

        protected static T Deserialize<T>(Stream stream, object context = null)
        {
#if TESTASYNC
            System.Threading.Tasks.Task<T> task = Serializer.DeserializeAsync<T>(stream, context);
            task.ConfigureAwait(false);
            task.Wait();
            return task.Result;
#else
            return Serializer.Deserialize<T>(stream, context);
#endif
        }

        protected static byte[] Serialize(object o)
        {
            MemoryStream stream = new();
            Serialize(stream, o);
            return stream.ToArray();
        }

        protected static void Serialize(Stream stream, object o, object context = null)
        {
#if TESTASYNC
            System.Threading.Tasks.Task task = Serializer.SerializeAsync(stream, o, context);
            task.ConfigureAwait(false);
            task.Wait();
#else
            Serializer.Serialize(stream, o, context);
#endif
        }

        protected static void SerializeBe(Stream stream, object o)
        {
#if TESTASYNC
            System.Threading.Tasks.Task task = SerializerBe.SerializeAsync(stream, o);
            task.ConfigureAwait(false);
            task.Wait();
#else
            SerializerBe.Serialize(stream, o);
#endif
        }

        protected static T DeserializeBe<T>(Stream stream)
        {
#if TESTASYNC
            System.Threading.Tasks.Task<T> task = SerializerBe.DeserializeAsync<T>(stream);
            task.ConfigureAwait(false);
            task.Wait();
            return task.Result;
#else
            return SerializerBe.Deserialize<T>(stream);
#endif
        }

        private static void PrintIndent(int depth)
        {
            string indent = new([.. Enumerable.Repeat(' ', depth * 4)]);
            Debug.Write(indent);
        }

        private static void PrintSerialize(Type type)
        {
            Debug.WriteLine($"S-{type}");
        }

        private static void PrintDeserialize(Type type)
        {
            Debug.WriteLine($"D-{type}");
        }

        private static void OnMemberSerializing(object sender, MemberSerializingEventArgs e)
        {
            PrintIndent(e.Context.Depth);
            Debug.WriteLine("S-Start: {0} @ {1}", e.MemberName, e.Offset);
        }

        private static void OnMemberSerialized(object sender, MemberSerializedEventArgs e)
        {
            PrintIndent(e.Context.Depth);
            object value = e.Value ?? "null";
            Debug.WriteLine("S-End: {0} ({1}) @ {2}", e.MemberName, value, e.Offset);
        }

        private static void OnMemberDeserializing(object sender, MemberSerializingEventArgs e)
        {
            PrintIndent(e.Context.Depth);
            Debug.WriteLine("D-Start: {0} @ {1}", e.MemberName, e.Offset);
        }

        private static void OnMemberDeserialized(object sender, MemberSerializedEventArgs e)
        {
            PrintIndent(e.Context.Depth);
            object value = e.Value ?? "null";

            if (value is byte[] byteArray)
            {
                value = BitConverter.ToString(byteArray);
            }

            Debug.WriteLine("D-End: {0} ({1}) @ {2}", e.MemberName, value, e.Offset);
        }
    }
}