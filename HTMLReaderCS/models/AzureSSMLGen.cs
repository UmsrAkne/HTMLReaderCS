using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models {
    public class AzureSSMLGen {

        private string RootStartTag => "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"string\">";
        private string RootEndTag => "</speak>";

        // voice に指定できる値は他にも存在するが、現状では他の声で読み上げてもらう予定はないので固定値とする。
        private string VoiceNameStartTag => "<voice name=\"ja-JP-KeitaNeural\">";
        private string VoiceNameEndTag => "</voice>";

        /// <summary>
        /// 読み上げのスピードを指定します。デフォルト値は 100 でそのままの速度。
        /// 50 なら半分の速度に、200 なら倍の速度となります。
        /// </summary>
        public int Rate { get; set; }
        public int DefaultRate => 100;

        public AzureSSMLGen() {
            Rate = DefaultRate;
        }

        public string getSSML(string plainText) {
            string prosodyStartTag = "";
            string prosodyEndTag = "";

            if(Rate != DefaultRate) {
                Decimal rateDecimal = Decimal.Divide(Rate,100);
                prosodyStartTag = $"<prosody rate=\"{rateDecimal}\">";
                prosodyEndTag = "</prosody>";
            }

            return $"{RootStartTag}" +
                       $"{VoiceNameStartTag}" +
                           $"{prosodyStartTag}" +
                               $"{plainText}" +
                           $"{prosodyEndTag}" +
                       $"{VoiceNameEndTag}" +
                   $"{RootEndTag}";
        }
    }
}
