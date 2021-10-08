namespace HTMLReaderCS.Models.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HashGeneratorTests
    {
        [TestMethod]
        public void GetMD5HashTest()
        {
            Assert.AreEqual(HashGenerator.GetMD5Hash("abcdefghijk").Length, 32);
        }
    }
}