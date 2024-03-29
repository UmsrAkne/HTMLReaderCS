﻿namespace HTMLReaderCS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    // 声の高さ
    public enum Pitch
    {
        Default, XLow, Low, Mediaum, High, XHigh
    }

    // 読み上げ速度
    public enum Rate
    {
        Default, XSlow, Slow, Medium, Fast, XFast
    }

    // 音量
    public enum Volume
    {
        Default, XSoft, Soft, Medium, Loud, XLoud, Silent, Minus10Db, Plus10Db
    }

    // 声の強弱
    public enum Emphasis
    {
        strong, moderate, none, reduced
    }

    // 声の一時停止の強弱
    public enum Break
    {
        None, XWeak, Medium, Strong, XStrong
    }

    /// <summary>
    /// テキストを SSML に変換する機能を提供します
    /// </summary>
    public class SSMLConverter
    {
        public const int DefaultVocalTractLength = 100;
        private int vocalTractLength;

        private Dictionary<Pitch, string> pitchStrings = new Dictionary<Pitch, string>
        {
            { Pitch.Default,    "\"default\"" },
            { Pitch.XLow,       "\"x-low\"" },
            { Pitch.Low,        "\"low\"" },
            { Pitch.Mediaum,    "\"medium\"" },
            { Pitch.High,       "\"high\"" },
            { Pitch.XHigh,      "\"x-high\"" }
        };

        private Dictionary<Rate, string> rateStrings = new Dictionary<Rate, string>
        {
            { Rate.Default,  "\"default\"" },
            { Rate.XSlow,    "\"x-slow\"" },
            { Rate.Slow,     "\"slow\"" },
            { Rate.Medium,   "\"medium\"" },
            { Rate.Fast,     "\"fast\"" },
            { Rate.XFast,    "\"x-fast\"" },
        };

        private Dictionary<Volume, string> volumeStrings = new Dictionary<Volume, string>
        {
            { Volume.Default,    "\"default\"" },
            { Volume.XSoft,      "\"x-soft\"" },
            { Volume.Soft,       "\"soft\"" },
            { Volume.Medium,     "\"medium\"" },
            { Volume.Loud,       "\"loud\"" },
            { Volume.XLoud,      "\"x-loud\"" },
            { Volume.Silent,     "\"silent\"" },
            { Volume.Minus10Db,  "\"-10dB\"" },
            { Volume.Plus10Db,   "\"+10bB\"" },
        };

        private Dictionary<Break, string> breakStrings = new Dictionary<Break, string>
        {
            { Break.None,    "\"none\"" },
            { Break.XWeak,   "\"x-weak\"" },
            { Break.Medium,  "\"medium\"" },
            { Break.Strong,  "\"strong\"" },
            { Break.XStrong, "\"x-strong\"" },
        };

        public SSMLConverter()
        {
            VocalTractLength = DefaultVocalTractLength;
        }

        public SSMLConverter(string text) : this()
        {
            Text = text;
        }

        public string Text { get; set; }

        public Pitch Pitch { get; set; } = Pitch.Default;

        public Rate Rate { get; set; } = Rate.Default;

        public Volume Volume { get; set; } = Volume.Default;

        public Emphasis Emphasis { get; set; } = Emphasis.none;

        public Break Break { get; set; } = Break.None;

        /// <summary>
        /// 声道の長さを指定します。デフォルト状態では DefaultVoiceTractLength の値が割り当てられます。
        /// 値は 50 - 200 の間の値を指定します。
        /// </summary>
        public int VocalTractLength
        {
            get => vocalTractLength;
            set
            {
                if (value < 50)
                {
                    vocalTractLength = 50;
                }
                else if (value > 200)
                {
                    vocalTractLength = 200;
                }
                else
                {
                    vocalTractLength = value;
                }
            }
        }

        /// <summary>
        /// getSSML() を実行した際、Textに含まれる改行文字を <break strength="xxx" /> に置き換えるかどうかを指定します。
        /// </summary>
        public bool DoReplaceNewLineToBreak { get; set; } = false;

        /// <summary>
        /// メソッド内部でSSMLを生成して返します
        /// </summary>
        /// <returns></returns>
        public string GetSSML()
        {
            string ssml = Text;

            if (DoReplaceNewLineToBreak)
            {
                string breakTag = "<break strength=" + breakStrings[this.Break] + " />";
                ssml = Regex.Replace(ssml, @"\r\n?|\n", breakTag);
            }

            if (VocalTractLength != DefaultVocalTractLength)
            {
                string vtlTag = "<amazon:effect vocal-tract-length=";
                vtlTag += "\"" + VocalTractLength.ToString() + "%\">";
                ssml = vtlTag + ssml + "</amazon:effect>";
            }

            if (!ProsodyIsDefault())
            {
                string prosodyTag = "<prosody ";
                prosodyTag += (this.Pitch != Pitch.Default) ? "pitch=" + pitchStrings[this.Pitch] + " " : string.Empty;
                prosodyTag += (this.Rate != Rate.Default) ? "rate=" + rateStrings[this.Rate] + " " : string.Empty;
                prosodyTag += (this.Volume != Volume.Default) ? "volume=" + volumeStrings[this.Volume] + " " : string.Empty;
                prosodyTag += ">";
                ssml = prosodyTag + ssml + "</prosody>";
            }

            if (this.Emphasis != Emphasis.none)
            {
                ssml = "<emphasis level=" + "\"" + this.Emphasis.ToString() + "\">" + ssml + "</emphasis>";
            }

            ssml = "<speak>" + ssml + "</speak>";

            return ssml;
        }

        public bool ProsodyIsDefault() =>
            this.Pitch == Pitch.Default && this.Rate == Rate.Default && this.Volume == Volume.Default;

        /// <summary>
        /// Prosody に関わるパラメーターである Pitch, Rate, Volume プロパティを全て default に設定します
        /// </summary>
        public void ResetProsody()
        {
            Pitch = Pitch.Default;
            Rate = Rate.Default;
            Volume = Volume.Default;
        }
    }
}
