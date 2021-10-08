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
    public class SSMLConverterTests
    {
        [TestMethod]
        public void SSMLConverterTest()
        {
            SSMLConverter converter = new SSMLConverter();
            converter.Text = "test";
            Assert.AreEqual(converter.GetSSML(), "<speak>test</speak>");

            Assert.IsTrue(converter.ProsodyIsDefault());
            converter.Volume = Volume.Medium;

            Assert.IsFalse(converter.ProsodyIsDefault());

            Assert.AreEqual(converter.GetSSML(), "<speak><prosody volume=\"medium\" >test</prosody></speak>");

            converter.Rate = Rate.XSlow;
            Assert.AreEqual(converter.GetSSML(), "<speak><prosody rate=\"x-slow\" volume=\"medium\" >test</prosody></speak>");

            converter.Pitch = Pitch.High;
            Assert.AreEqual(converter.GetSSML(), "<speak><prosody pitch=\"high\" rate=\"x-slow\" volume=\"medium\" >test</prosody></speak>");

            converter.ResetProsody();
            Assert.IsTrue(converter.ProsodyIsDefault());

            converter.Emphasis = Emphasis.strong;
            Assert.AreEqual(converter.GetSSML(), "<speak><emphasis level=\"strong\">test</emphasis></speak>");

            converter.Rate = Rate.Slow;
            Assert.AreEqual(converter.GetSSML(), "<speak><emphasis level=\"strong\"><prosody rate=\"slow\" >test</prosody></emphasis></speak>");

            converter.DoReplaceNewLineToBreak = true;
            converter.ResetProsody();
            converter.Emphasis = Emphasis.none;
            converter.Break = Break.Medium;
            converter.Text = "test\rtest\r\ntest\ntest";
            Assert.AreEqual(converter.GetSSML(), "<speak>test<break strength=\"medium\" />test<break strength=\"medium\" />test<break strength=\"medium\" />test</speak>");

            converter = new SSMLConverter("test");
            converter.VocalTractLength = 120;

            string expectedString = "<speak>";
            expectedString += "<amazon:effect vocal-tract-length=\"120%\">test</amazon:effect>";
            expectedString += "</speak>";

            Assert.AreEqual(converter.GetSSML(), expectedString);

            converter.DoReplaceNewLineToBreak = true;
            converter.Break = Break.Medium;
            converter.Rate = Rate.Fast;
            converter.Text = "test\r\ntest";

            // max vocalTractLength
            converter.VocalTractLength = 250;
            Assert.AreEqual(converter.VocalTractLength, 200);

            // min vocalTractLength
            converter.VocalTractLength = 0;
            Assert.AreEqual(converter.VocalTractLength, 50);

            expectedString = "<speak>" +
                                "<prosody rate=\"fast\" >" +
                                    "<amazon:effect vocal-tract-length=\"50%\">" +
                                        "test" + "<break strength=\"medium\" />" + "test" +
                                    "</amazon:effect>" +
                                "</prosody>" +
                            "</speak>";

            Assert.AreEqual(converter.GetSSML(), expectedString);
        }
    }
}