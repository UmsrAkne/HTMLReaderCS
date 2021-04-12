using Microsoft.VisualStudio.TestTools.UnitTesting;
using HTMLReaderCS.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models.Tests {
    [TestClass()]
    public class PollyPlayerTests {
        [TestMethod()]
        public void PollyPlayerTest() {
            var pollyPlayer = new PollyPlayer();

        }

        [TestMethod()]
        public void ssmlTalkTest() {
            var pp = new PollyPlayer();
            pp.ssmlTalk("読み上げテキストです");
        }
    }
}