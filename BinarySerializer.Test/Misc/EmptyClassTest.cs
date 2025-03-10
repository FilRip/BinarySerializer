using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class EmptyClassTest : TestBase
    {
        [TestMethod()]
        public void RoundtripEmptyClassTest()
        {
            var actual = Roundtrip(new EmptyClass());
            Assert.IsInstanceOfType<EmptyClass>(actual);
        }
    }
}