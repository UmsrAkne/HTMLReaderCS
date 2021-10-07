namespace HTMLReaderCS.models.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HTMLReaderCS.models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AzureTalkerTests
    {
        [TestMethod]
        public void m1Test()
        {
            var azureTalker = new AzureTalker();
            var ssmlGen = new AzureSSMLGen();
            azureTalker.ssmlTalk(ssmlGen.getSSML("このテキストを読み上げます"));
        }
    }
}