﻿namespace HTMLReaderCS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AzureSSMLGen
    {
        public AzureSSMLGen()
        {
            Rate = DefaultRate;
        }

        /// <summary>
        /// 読み上げのスピードを指定します。デフォルト値は 100 でそのままの速度。
        /// 50 なら半分の速度に、200 なら倍の速度となります。
        /// </summary>
        public int Rate { get; set; }

        public int DefaultRate => 100;

        /// <summary>
        /// ssml を生成する際、テキスト冒頭にウェイトを挿入します。時間指定は最大で 5000ms です。
        /// </summary>
        public TimeSpan BeforeWait { get; set; }

        /// <summary>
        /// ssml を生成する際、テキスト末尾にウェイトを挿入します。時間指定は最大で 5000ms です。
        /// </summary>
        public TimeSpan AfterWait { get; set; }

        private string RootStartTag => "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"string\">";

        private string RootEndTag => "</speak>";

        // voice に指定できる値は他にも存在するが、現状では他の声で読み上げてもらう予定はないので固定値とする。
        private string VoiceNameStartTag => "<voice name=\"ja-JP-KeitaNeural\">";

        private string VoiceNameEndTag => "</voice>";

        public string GetSSML(string plainText)
        {
            string prosodyStartTag = string.Empty;
            string prosodyEndTag = string.Empty;

            if (Rate != DefaultRate)
            {
                decimal rateDecimal = decimal.Divide(Rate, 100);
                prosodyStartTag = $"<prosody rate=\"{rateDecimal}\">";
                prosodyEndTag = "</prosody>";
            }

            string beforeWaitTag = string.Empty;
            string afterWaitTag = string.Empty;

            if (BeforeWait.TotalMilliseconds != 0)
            {
                beforeWaitTag = $"<break time=\"{BeforeWait.TotalMilliseconds}\" />";
            }

            if (AfterWait.TotalMilliseconds != 0)
            {
                afterWaitTag = $"<break time=\"{AfterWait.TotalMilliseconds}\" />";
            }

            return $"{RootStartTag}" +
                       $"{VoiceNameStartTag}" +
                           $"{beforeWaitTag}" +
                               $"{prosodyStartTag}" +
                                   $"{plainText}" +
                               $"{prosodyEndTag}" +
                            $"{afterWaitTag}" +
                       $"{VoiceNameEndTag}" +
                   $"{RootEndTag}";
        }
    }
}
