namespace HTMLReaderCS.models.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HashGeneratorTests
    {
        [TestMethod]
        public void getMD5HashTest()
        {
            Assert.AreEqual(HashGenerator.getMD5Hash("abcdefghijk").Length, 32);
        }
    }
}