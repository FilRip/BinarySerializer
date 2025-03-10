using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class MiscTests : TestBase
    {
        [TestMethod()]
#pragma warning disable S2699 // Tests should include assertions
        public async Task DontFlushTooMuchTest()
#pragma warning restore S2699 // Tests should include assertions
        {
            var serializer = new BinarySerializer();
            var expected = new DontFlushTooMuchClass();
            var stream = new UnflushableStream();

#pragma warning disable S6966 // Awaitable method should be used
            serializer.Serialize(stream, expected);
#pragma warning restore S6966 // Awaitable method should be used
            await serializer.SerializeAsync(stream, expected);
        }
    }
}
