using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

//using System.Runtime.Serialization.Formatters.Binary;

namespace BinarySerializer.Performance
{
    internal static class Program
    {
#pragma warning disable IDE0060 // Supprimer le paramètre inutilisé
        internal static void Main(string[] args)
#pragma warning restore IDE0060 // Supprimer le paramètre inutilisé
        {
            Beer beer = new()
            {
                Alcohol = 6,

                Brand = "Brand",
                Sort =
                [
                    new() {Name = "some sort of beer"},
                    new() {Name = "another beer"}
                ],
                WeirdNumber = 3,
                WeirdNumber2 = 2,
                Color = Color.Blue,
                TerminatedString = "hello everyone",
                Brewery = "Brasserie Grain d'Orge"
            };

            DoBS(beer, 100000);
            Task task = DoBSAsync(beer, 100000);
            task.Wait();
            DoBSParallel(beer, 100000);
            //DoBF(beer, 100000);
            Console.ReadKey();
        }

        private static void DoBS<T>(T obj, int iterations)
        {
            Stopwatch stopwatch = new();

            BinarySerialization.BinarySerializer ser = new();

            using (MemoryStream ms = new())
            {
                stopwatch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    ser.Serialize(ms, obj);
                }
                stopwatch.Stop();
                Console.WriteLine("BS SER: {0}", stopwatch.Elapsed);
                stopwatch.Reset();
            }

            MemoryStream dataStream = new();
            ser.Serialize(dataStream, obj);
            byte[] data = dataStream.ToArray();

            using (MemoryStream ms = new(data))
            {
                stopwatch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    ser.Deserialize<T>(ms);
                    ms.Position = 0;
                }
                stopwatch.Stop();
                Console.WriteLine("BS DESER: {0}", stopwatch.Elapsed);
                stopwatch.Reset();
            }
        }

        private static async Task DoBSAsync<T>(T obj, int iterations)
        {
            Stopwatch stopwatch = new();

            BinarySerialization.BinarySerializer ser = new();

            using (MemoryStream ms = new())
            {
                stopwatch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    await ser.SerializeAsync(ms, obj);
                }
                stopwatch.Stop();
                Console.WriteLine("BSA SER: {0}", stopwatch.Elapsed);
                stopwatch.Reset();
            }

            MemoryStream dataStream = new();
            await ser.SerializeAsync(dataStream, obj);
            byte[] data = dataStream.ToArray();

            using (MemoryStream ms = new(data))
            {
                stopwatch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    await ser.DeserializeAsync<T>(ms)
                        .ConfigureAwait(false);
                    ms.Position = 0;
                }
                stopwatch.Stop();
                Console.WriteLine("BSA DESER: {0}", stopwatch.Elapsed);
                stopwatch.Reset();
            }
        }

        private static void DoBSParallel<T>(T obj, int iterations)
        {
            Stopwatch stopwatch = new();

            BinarySerialization.BinarySerializer ser = new();

            stopwatch.Start();
            byte[][] data = Enumerable.Range(0, iterations).AsParallel().Select(i =>
            {
                using MemoryStream ms = new();
                ser.Serialize(ms, obj);
                return ms.ToArray();
            }).ToArray();

            stopwatch.Stop();
            Console.WriteLine("BS || SER: {0}", stopwatch.Elapsed);
            stopwatch.Reset();

            stopwatch.Start();
            data.AsParallel().ForAll(d => ser.Deserialize<T>(d));
            stopwatch.Stop();
            Console.WriteLine("BS || DESER: {0}", stopwatch.Elapsed);
            stopwatch.Reset();
        }

        // BinaryFormatter will be available again in later versions of .NET Core
        //private static void DoBF(object obj, int iterations)
        //{
        //    var formatter = new BinaryFormatter();

        //    var stopwatch = new Stopwatch();

        //    using (var ms = new MemoryStream())
        //    {
        //        stopwatch.Start();
        //        for (int i = 0; i < iterations; i++)
        //        {
        //            formatter.Serialize(ms, obj);
        //        }
        //        stopwatch.Stop();
        //        Console.WriteLine("BF SER: {0}", stopwatch.Elapsed);
        //        stopwatch.GetInitial();
        //    }

        //    var dataStream = new MemoryStream();
        //    formatter.Serialize(dataStream, obj);
        //    byte[] data = dataStream.ToArray();

        //    using (var ms = new MemoryStream(data))
        //    {
        //        stopwatch.Start();
        //        for (int i = 0; i < iterations; i++)
        //        {
        //            formatter.Deserialize(ms);
        //            ms.Position = 0;
        //        }
        //        stopwatch.Stop();
        //        Console.WriteLine("BF DESER: {0}", stopwatch.Elapsed);
        //        stopwatch.GetInitial();
        //    }
        //}
    }
}