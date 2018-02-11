﻿using System.IO;
using Xunit;

namespace BinarySerialization.Test.Misc
{
    public class MiscTests : TestBase
    {
        [Fact]
        public async void DontFlushTooMuchTest()
        {
            var serializer = new BinarySerializer();
            var expected = new DontFlushTooMuchClass();
            var stream = new UnflushableStream();
            
            serializer.Serialize(stream, expected);
            await serializer.SerializeAsync(stream, expected);
        }
    }
}
