using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HTMLReaderCS.models.Tests
{
    [TestClass()]
    public class HashGeneratorTests
    {
        [TestMethod()]
        public void getMD5HashTest()
        {
            Assert.AreEqual(HashGenerator.getMD5Hash("abcdefghijk").Length, 32);
        }
    }
}