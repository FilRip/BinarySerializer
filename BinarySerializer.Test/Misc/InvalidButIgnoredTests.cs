using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Misc
{
    [TestClass()]
    public class InvalidButIgnoredTests : TestBase
    {
        [TestMethod()]
        public void InvalidButIgnoredTest()
        {
            var actual = Roundtrip(new InvalidButIgnoredContainerClass
            {
                InvalidButIgnored = new InvalidButIgnoredTypeClass()
            });
            Assert.IsInstanceOfType<InvalidButIgnoredContainerClass>(actual);
            Assert.IsNull(actual.InvalidButIgnored);
        }
    }
}
