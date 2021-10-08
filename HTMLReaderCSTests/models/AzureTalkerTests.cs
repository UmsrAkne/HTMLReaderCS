namespace HTMLReaderCS.Models.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HTMLReaderCS.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AzureTalkerTests
    {
        [TestMethod]
        public void M1Test()
        {
            var azureTalker = new AzureTalker();
            var ssmlGen = new AzureSSMLGen();
            azureTalker.SSMLTalk(ssmlGen.GetSSML("このテキストを読み上げます"));
        }
    }
}