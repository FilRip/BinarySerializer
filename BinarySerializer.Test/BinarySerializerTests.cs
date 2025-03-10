using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using BinarySerialization.Constants;
using BinarySerialization.CustomEventArgs;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test
{
    [TestClass()]
    public class BinarySerializerTests : TestBase
    {
        private const string Disclaimer = "This isn't really cereal";
        private readonly BinarySerializer _serializer = new();

        public BinarySerializerTests()
        {
            _serializer.MemberDeserialized += SerializerMemberDeserialized;
            _serializer.MemberSerialized += SerializerMemberSerialized;
        }

        private static Cereal Cerealize()
        {
            MemoryStream disclaimerStream = new();
            StreamWriter writer = new(disclaimerStream);
            writer.Write(Disclaimer);
            writer.Flush();
            disclaimerStream.Position = 0;

            return new Cereal
            {
                IsLittleEndian = "false",
                Name = "Cheerios",
                Manufacturer = "General Mills",
                NutritionalInformation = new NutritionalInformation
                {
                    Calories = 100,
                    Fat = 1.5f,
                    Cholesterol = 64,
                    VitaminA = 2,
                    VitaminB = 3,
                    OtherNestedStuff = ["it's", "got", "electrolytes"],
                    OtherNestedStuff2 = ["stuff", "plants", "crave"],
                    Toys =
                    [
                        new("Truck"),
                        new("Godzilla"),
                        new("EZ Bake Oven"),
                        new("Bike", true)
                    ],
                    WeirdOutlierLengthedField = "hihihihihi",
                    Ingredients = new Ingredients
                    {
                        MainIngredient = new Iron()
                    }
                },
                DoubleField = 33.33333333,
                OtherStuff = ["apple", "pear", "banana"],
                Shape = CerealShape.Circular,
                DefinitelyNotTheShape = CerealShape.Square,
                DontSerializeMe = "bro",
                SerializeMe = "!",
                Outlier = 0,
                ExplicitlyTerminatedList = ["red", "white", "blue"],
                ImplicitlyTerminatedList =
                    [CerealShape.Circular, CerealShape.Circular, CerealShape.Square],
                ArrayOfInts = [1, 2, 3],
                InvalidFieldLength = "oops",
                DisclaimerLength = disclaimerStream.Length,
                Disclaimer = disclaimerStream
            };
        }

        [TestMethod()]
        public void ParallelCerealTest()
        {
            System.Collections.Generic.IEnumerable<int> runs = Enumerable.Range(0, 1000);
            Parallel.ForEach(runs, i => CerealTest());
        }

        [TestMethod()]
        public void CerealTest()
        {
            Cereal cereal = Cerealize();


            using MemoryStream stream = new();
            _serializer.Serialize(stream, cereal);
            stream.Position = 0;

            Assert.AreEqual(Endianness.Big, _serializer.Endianness);

            //File.WriteAllBytes("c:\\temp\\out.bin", stream.ToArray());


            Cereal cereal2 = _serializer.Deserialize<Cereal>(stream);

            Assert.AreEqual("Cheeri", cereal2.Name);
            Assert.AreEqual(cereal.Manufacturer, cereal2.Manufacturer);
            Assert.AreEqual(cereal.NutritionalInformation.Fat, cereal2.NutritionalInformation.Fat);
            Assert.AreEqual(cereal.NutritionalInformation.Calories, cereal2.NutritionalInformation.Calories);
            Assert.AreEqual(cereal.NutritionalInformation.VitaminA, cereal2.NutritionalInformation.VitaminA);
            Assert.AreEqual(cereal.NutritionalInformation.VitaminB, cereal2.NutritionalInformation.VitaminB);
            Assert.IsTrue(cereal.NutritionalInformation.OtherNestedStuff.SequenceEqual(
                cereal2.NutritionalInformation.OtherNestedStuff));
            Assert.IsTrue(cereal.NutritionalInformation.OtherNestedStuff2.SequenceEqual(
                cereal2.NutritionalInformation.OtherNestedStuff2));

            Assert.IsTrue(cereal.NutritionalInformation.Toys.SequenceEqual(cereal2.NutritionalInformation.Toys));

            Assert.IsTrue(cereal.NutritionalInformation.Ingredients.MainIngredient is Iron);

            Assert.AreEqual(cereal2.DoubleField, cereal2.DoubleField);
            CollectionAssert.Contains(cereal2.OtherStuff, "app");
            CollectionAssert.Contains(cereal2.OtherStuff, "pea");
            CollectionAssert.Contains(cereal2.OtherStuff, "ban");
            Assert.AreEqual(3, cereal2.OtherStuff.Count);
            Assert.AreEqual(cereal2.OtherStuff.Count, cereal2.OtherStuffCount);
            Assert.AreEqual(CerealShape.Circular, cereal2.Shape);
            Assert.AreEqual(CerealShape.Square, cereal2.DefinitelyNotTheShape);
            Assert.IsNull(cereal2.DontSerializeMe);
            Assert.AreEqual(cereal.SerializeMe, cereal2.SerializeMe);
            Assert.AreEqual(3, cereal2.ArrayOfInts.Length);
            Assert.AreEqual(1, cereal2.ArrayOfInts[0]);
            Assert.AreEqual(2, cereal2.ArrayOfInts[1]);
            Assert.AreEqual(3, cereal2.ArrayOfInts[2]);
            Assert.AreEqual(cereal.NutritionalInformation.WeirdOutlierLengthedField.Length / 2.0, cereal2.Outlier);

            Assert.IsTrue(cereal.ExplicitlyTerminatedList.SequenceEqual(cereal2.ExplicitlyTerminatedList));
            Assert.IsTrue(cereal.ImplicitlyTerminatedList.SequenceEqual(cereal2.ImplicitlyTerminatedList));

            StreamReader reader = new(cereal2.Disclaimer);
            Assert.AreEqual(Disclaimer, reader.ReadToEnd());
        }


        private void SerializerMemberSerialized(object sender, MemberSerializedEventArgs e)
        {
            if (e.MemberName == "IsLittleEndian")
            {
                bool isLittleEndian = bool.Parse((string)e.Value);
                if (!isLittleEndian)
                    _serializer.Endianness = Endianness.Big;
            }

            Debug.WriteLine("write {0}: {1} @ {2}", e.MemberName, e.Value, e.Offset);
        }

        private void SerializerMemberDeserialized(object sender, MemberSerializedEventArgs e)
        {
            if (e.MemberName == "IsLittleEndian")
            {
                bool isLittleEndian = bool.Parse((string)e.Value);
                if (!isLittleEndian)
                    _serializer.Endianness = Endianness.Big;
            }

            Debug.WriteLine("read {0}: {1} @ {2}", e.MemberName, e.Value, e.Offset);
        }

        [TestMethod()]
        public void NonSeekableStreamSerializationTest()
        {
            NonSeekableStream stream = new();
            BinarySerializer serializer = new();
            serializer.Serialize(stream, new Iron());
            Assert.IsNull(stream.GetStream());
        }

        [TestMethod()]
        public void NonSeekableStreamWithOffsetAttributeShouldThrowInvalidOperationException()
        {
            NonSeekableStream stream = new();
            BinarySerializer serializer = new();
            Assert.ThrowsExactly<InvalidOperationException>(() => serializer.Serialize(stream, Cerealize()));
        }

        [TestMethod()]
        public void NullStreamSerializationShouldThrowArgumentNullException()
        {
            BinarySerializer serializer = new();
            Assert.ThrowsExactly<ArgumentNullException>(() => serializer.Serialize(null, new object()));
        }

        [TestMethod()]
        public void NullGraphSerializationShouldSerializeNothing()
        {
            BinarySerializer serializer = new();
            MemoryStream stream = new();
            serializer.Serialize(stream, null);
            Assert.AreEqual(0, stream.Length);
        }

        //        [TestMethod()]
        //#if DEBUG
        //        [ExpectedException(typeof(ArgumentNullException))]
        //#else
        //        [ExpectedException(typeof(InvalidOperationException))]
        //#endif
        //        public void NullMemberSerializationShouldThrowException()
        //        {
        //            var serializer = new BinarySerialization.BinarySerializer();
        //            serializer.Serialize(new MemoryStream(), new NullArrayClass());
        //        }

        [TestMethod()]
        public void UnresolvedSubtypeMemberDeserializationYieldsNull()
        {
            BinarySerializer serializer = new();
            Ingredients ingredients = serializer.Deserialize<Ingredients>([0x4]);
            Assert.IsNull(ingredients.MainIngredient);
        }

        [TestMethod()]
        public void ImplicitTermination()
        {
            byte[] data = [0x0, 0x1, 0x2, 0x3];

            BinarySerializer serializer = new();
            ImplictTermination byteList = serializer.Deserialize<ImplictTermination>(data);

            Assert.AreEqual(4, byteList.Data.Count);
        }

        //}
        //    serializer.Deserialize<List<string>>(new byte[3]);
        //    var serializer = new BinarySerialization.BinarySerializer();
        //{
        //public void CollectionAtRootShouldThrowNotSupportedException()
        //[ExpectedException(typeof(NotSupportedException))]

        //[TestMethod()]
    }
}