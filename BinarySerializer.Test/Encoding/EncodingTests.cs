using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Encoding
{
    [TestClass()]
    public class EncodingTests : TestBase
    {
        public EncodingTests()
        {
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [TestMethod()]
        public void EncodingTest()
        {
            EncodingClass expected = new() { Name = "السلام عليكم" };
            byte[] expectedData = System.Text.Encoding.GetEncoding("windows-1256").GetBytes(expected.Name + char.MinValue);
            EncodingClass actual = Roundtrip(expected, expectedData);

            Assert.AreEqual(expected.Name, actual.Name);
        }

        [TestMethod()]
        public void EncodingTest2()
        {
            EncodingClass2 expected = new() { Name = "السلام عليكم" };
            byte[] expectedData = System.Text.Encoding.GetEncoding("windows-1256").GetBytes(expected.Name + char.MinValue);
            EncodingClass2 actual = Roundtrip(expected, expectedData);

            Assert.AreEqual(expected.Name, actual.Name);
        }

        [TestMethod()]
        public void FieldEncodingTest()
        {
            FieldEncodingClass expected = new() { Value = "السلام عليكم", Encoding = "windows-1256" };
            byte[] encodingFieldData = System.Text.Encoding.UTF8.GetBytes(expected.Encoding + char.MinValue);
            byte[] expectedValueData = System.Text.Encoding.GetEncoding(expected.Encoding).GetBytes(expected.Value + char.MinValue);

            byte[] expectedData = [.. encodingFieldData, .. expectedValueData];

            FieldEncodingClass actual = Roundtrip(expected, expectedData);

            Assert.AreEqual(expected.Encoding, actual.Encoding);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void ConstFieldEncodingTest()
        {
            ConstEncodingClass expected = new() { Value = "السلام عليكم" };
            byte[] expectedData = System.Text.Encoding.GetEncoding("windows-1256").GetBytes(expected.Value + char.MinValue);
            ConstEncodingClass actual = Roundtrip(expected, expectedData);

            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod()]
        public void NullTerminatedUtf16Test()
        {
            EncodingClassUtf16 expected = new() { Name = "hello" };
            byte[] expectedData = System.Text.Encoding.Unicode.GetBytes(expected.Name + char.MinValue);
            EncodingClassUtf16 actual = Roundtrip(expected, expectedData);

            Assert.AreEqual(expected.Name, actual.Name);
        }
    }
}