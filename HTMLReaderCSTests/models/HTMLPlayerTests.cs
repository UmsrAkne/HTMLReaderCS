using Microsoft.VisualStudio.TestTools.UnitTesting;
using HTMLReaderCS.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTMLReaderCSTests.models;

namespace HTMLReaderCS.models.Tests
{
    [TestClass()]
    public class HTMLPlayerTests
    {
        [TestMethod()]
        public void HTMLPlayerTest() {
            var dummyTalker = new DummyTalker();
            HTMLPlayer htmlPlayer = new HTMLPlayer(dummyTalker);

            HTMLContents htmlText = new HTMLContents(
                "<html>" +
                    "<body>" +
                        "<title>titleText</title>" +
                        "<p>testText</p>" +
                        "<p>2testText2</p>" +
                    "</body>" +
                "</html>"
            );

            htmlPlayer.HtmlContents = htmlText;
            htmlPlayer.PlayCommand.Execute();
            Assert.IsTrue(dummyTalker.PlayingText.Contains(">titleText<"));
            Assert.AreEqual(htmlPlayer.PlayingPlainText, "titleText");
            dummyTalker.dispatchTalkEnded();

            Assert.IsTrue(dummyTalker.PlayingText.Contains(">testText<"));
            Assert.AreEqual(htmlPlayer.PlayingPlainText, "testText");
            dummyTalker.dispatchTalkEnded();

            Assert.IsTrue(dummyTalker.PlayingText.Contains(">2testText2<"));
            Assert.AreEqual(htmlPlayer.PlayingPlainText, "2testText2");
            dummyTalker.dispatchTalkEnded();

        }
    }
}