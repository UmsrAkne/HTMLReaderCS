using Microsoft.VisualStudio.TestTools.UnitTesting;
using HTMLReaderCS.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models.Tests
{
    [TestClass()]
    public class AzureTalkerTests
    {
        [TestMethod()]
        public void m1Test()
        {
            var azureTalker = new AzureTalker();
            var ssmlGen = new AzureSSMLGen();
            azureTalker.ssmlTalk(ssmlGen.getSSML("このテキストを読み上げます"));
        }
    }
}