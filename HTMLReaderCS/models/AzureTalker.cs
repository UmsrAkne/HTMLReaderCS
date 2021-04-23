using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using WMPLib;

namespace HTMLReaderCS.models {
    public class AzureTalker : ITalker {

        public string OutputFileName { get; private set; }
        private WindowsMediaPlayer WMP { get; } = new WindowsMediaPlayer();

        public event EventHandler TalkEnded;

        private DirectoryInfo OutputDirectoryInfo = new DirectoryInfo("outputs");

        public AzureTalker() {
            if (!OutputDirectoryInfo.Exists) {
                OutputDirectoryInfo.Create();
            }
        }

        public async void ssmlTalk(string ssmlText) {
            await talk(ssmlText);
        }

        public void stop() {
            WMP.controls.stop();
        }

        private async Task talk(string ssml) {

            string key = Environment.GetEnvironmentVariable("Microsoft_Speech_Secret_key");

            var config = SpeechConfig.FromSubscription(key, "japaneast");
            config.SpeechSynthesisLanguage = "ja-JP";
            config.SpeechSynthesisVoiceName = "ja-JP-KeitaNeural";

            OutputFileName = DateTime.Now.ToString("yyyyMMddHHmmssff") + ".wav";
            var audioConfig = AudioConfig.FromWavFileOutput($"{OutputDirectoryInfo.Name}\\{OutputFileName}");

            using (var synthesizer = new SpeechSynthesizer(config,audioConfig)) {
                var ssmlGen = new AzureSSMLGen();
                await synthesizer.SpeakSsmlAsync(ssml);

            }

            WMP.URL = $"{OutputDirectoryInfo.Name}\\{OutputFileName}";
            WMP.controls.play();
        }

    }
}
