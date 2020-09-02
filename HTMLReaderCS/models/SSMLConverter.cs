using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models
{
    /// <summary>
    /// テキストを SSML に変換する機能を提供します
    /// </summary>
    public class SSMLConverter {

        public String Text { get; set; }

        public Pitch Pitch { get; set; } = Pitch.Default;
        public Rate Rate { get; set; } = Rate.Default;
        public Volume Volume{ get; set; } = Volume.Default;

        public SSMLConverter() {
        }

        public SSMLConverter(String text) : this() {
            Text = text;
        }

        /// <summary>
        /// メソッド内部でSSMLを生成して返します
        /// </summary>
        /// <returns></returns>
        public String getSSML() {
            String ssml = Text;
            if (!prosodyIsDefault()) {
                ssml = "<prosody ";
                ssml += (this.Pitch != Pitch.Default) ? "pitch=" + pitchStrings[this.Pitch] + " " : "";
                ssml += (this.Rate != Rate.Default) ? "rate=" + rateStrings[this.Rate] + " " : "";
                ssml += (this.Volume != Volume.Default) ? "volume=" + volumeStrings[this.Volume] + " " : "";
                ssml += ">" + Text + "</prosody>";
            }

            ssml = "<speak>" + ssml + "</speak>";

            return ssml;
        }

        public bool prosodyIsDefault() =>
            this.Pitch == Pitch.Default && this.Rate == Rate.Default && this.Volume == Volume.Default;

        /// <summary>
        /// Prosody に関わるパラメーターである Pitch, Rate, Volume プロパティを全て default に設定します
        /// </summary>
        public void resetProsody() {
            Pitch = Pitch.Default;
            Rate = Rate.Default;
            Volume = Volume.Default;
        }

        private Dictionary<Pitch, String> pitchStrings = new Dictionary<Pitch, string> {
            {Pitch.Default, "\"default\"" },
            {Pitch.XLow,    "\"x-low\"" },
            {Pitch.Low,     "\"low\"" },
            {Pitch.Mediaum, "\"medium\"" },
            {Pitch.High,    "\"high\"" },
            {Pitch.XHigh,   "\"x-high\"" },
        };

        private Dictionary<Rate, String> rateStrings = new Dictionary<Rate, string> {
            {Rate.Default,  "\"default\"" },
            {Rate.XSlow,    "\"x-slow\"" },
            {Rate.Slow,     "\"slow\"" },
            {Rate.Medium,   "\"medium\"" },
            {Rate.Fast,     "\"fast\"" },
            {Rate.XFast,    "\"x-fast\"" },
        };

        private Dictionary<Volume, String> volumeStrings = new Dictionary<Volume, string> {
            {Volume.Default,    "\"default\"" },
            {Volume.XSoft,      "\"x-soft\"" },
            {Volume.Soft,       "\"soft\"" },
            {Volume.Medium,     "\"medium\"" },
            {Volume.Loud,       "\"loud\"" },
            {Volume.XLoud,      "\"x-loud\"" },
            {Volume.Silent,     "\"silent\"" },
            {Volume.Minus10Db,  "\"-10dB\"" },
            {Volume.Plus10Db,   "\"+10bB\"" },
        };

    }

    // 声の高さ
    public enum Pitch {
        Default, XLow, Low, Mediaum, High, XHigh 
    }

    // 読み上げ速度
    public enum Rate {
        Default, XSlow, Slow, Medium, Fast, XFast
    }

    // 音量
    public enum Volume {
        Default, XSoft, Soft, Medium, Loud, XLoud, Silent, Minus10Db, Plus10Db
    }
}
