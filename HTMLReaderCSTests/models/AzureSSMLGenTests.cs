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
    public class AzureSSMLGenTests
    {
        [TestMethod()]
        public void getSSMLTest()
        {
            var gen = new AzureSSMLGen();
            gen.Rate = 80;

            var ssmlActual = gen.getSSML("読み上げるテキスト");

            string expected = "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"string\">" +
                                  "<voice name=\"ja-JP-KeitaNeural\">" +
                                    "<prosody rate=\"0.8\">" +
                                        "読み上げるテキスト" +
                                    "</prosody>" +
                                  "</voice>" +
                              "</speak>";

            Assert.AreEqual(ssmlActual, expected);

            gen.BeforeWait = new TimeSpan(0, 0, 0, 0, 1000);
            gen.AfterWait = new TimeSpan(0, 0, 0, 0, 2000);

            string expected2 = "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"string\">" +
                                  "<voice name=\"ja-JP-KeitaNeural\">" +
                                    "<break time=\"1000\" />" +
                                        "<prosody rate=\"0.8\">" +
                                            "読み上げるテキスト" +
                                        "</prosody>" +
                                    "<break time=\"2000\" />" +
                                  "</voice>" +
                              "</speak>";

            Assert.AreEqual(gen.getSSML("読み上げるテキスト"), expected2);
        }
    }
}