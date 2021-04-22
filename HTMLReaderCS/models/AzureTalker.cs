using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace HTMLReaderCS.models {
    public class AzureTalker : ITalker {

        public string OutputFileName => "defalt";

        public event EventHandler TalkEnded;

        public void ssmlTalk(string ssmlText) {
            throw new NotImplementedException();
        }

        public void stop() {
            throw new NotImplementedException();
        }

        public async void m1() {
            await m();
        }

        private async Task m() {

            string key= Environment.GetEnvironmentVariable("Microsoft_Speech_Secret_key");

            var config = SpeechConfig.FromSubscription(key, "japaneast");
            config.SpeechSynthesisLanguage = "ja-JP";
            config.SpeechSynthesisVoiceName = "ja-JP-KeitaNeural";

            using (var synthesizer = new SpeechSynthesizer(config)) {

                var ssmlGen = new AzureSSMLGen();
                ssmlGen.Rate = 50;

                await synthesizer.SpeakSsmlAsync(ssmlGen.getSSML("このテキストを読み上げます"));
            }

        }

    }
}
