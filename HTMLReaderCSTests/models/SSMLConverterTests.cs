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
    public class SSMLConverterTests
    {
        [TestMethod()]
        public void SSMLConverterTest() {
            SSMLConverter converter = new SSMLConverter();
            converter.Text = "test";
            Assert.AreEqual(converter.getSSML(),"<speak>test</speak>");

            Assert.IsTrue(converter.prosodyIsDefault());
            converter.Volume = Volume.Medium;

            Assert.IsFalse(converter.prosodyIsDefault());

            Assert.AreEqual(converter.getSSML(), "<speak><prosody volume=\"medium\" >test</prosody></speak>");

            converter.Rate = Rate.XSlow;
            Assert.AreEqual(converter.getSSML(), "<speak><prosody rate=\"x-slow\" volume=\"medium\" >test</prosody></speak>");

            converter.Pitch = Pitch.High;
            Assert.AreEqual(converter.getSSML(), "<speak><prosody pitch=\"high\" rate=\"x-slow\" volume=\"medium\" >test</prosody></speak>");

            converter.resetProsody();
            Assert.IsTrue(converter.prosodyIsDefault());

            converter.Emphasis = Emphasis.strong;
            Assert.AreEqual(converter.getSSML(), "<speak><emphasis level=\"strong\">test</emphasis></speak>");

            converter.Rate = Rate.Slow;
            Assert.AreEqual(converter.getSSML(), "<speak><emphasis level=\"strong\"><prosody rate=\"slow\" >test</prosody></emphasis></speak>");
        }
    }
}