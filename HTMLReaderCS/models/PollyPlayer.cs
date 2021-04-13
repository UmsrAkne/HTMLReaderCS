using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Polly;
using Amazon.Polly.Model;
using System.Windows;
using WMPLib;

namespace HTMLReaderCS.models {
    public class PollyPlayer : ITalker {

        public SSMLConverter SsmlConverter { get; set; } = new SSMLConverter();

        private DirectoryInfo OutputDirectoryInfo { get; set; } = new DirectoryInfo("outputs");

        private AmazonPollyClient PollyClient { get; set; }

        private WindowsMediaPlayer WMP { get; set; } = new WindowsMediaPlayer();

        public event EventHandler TalkEnded;

        public PollyPlayer() {
            var accKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var secKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            PollyClient = new AmazonPollyClient(accKey,secKey, Amazon.RegionEndpoint.APNortheast1 );

            if (!OutputDirectoryInfo.Exists) {
                OutputDirectoryInfo.Create();
            }
        }

        public void ssmlTalk(string ssmlText) {

            var req = new SynthesizeSpeechRequest();
            req.VoiceId = VoiceId.Takumi;
            req.OutputFormat = OutputFormat.Mp3;
            req.TextType = TextType.Ssml;
            req.Text = ssmlText;

            var response = PollyClient.SynthesizeSpeech(req);

            var stream = response.AudioStream;

            var outputFileName = DateTime.Now.ToString("yyyyMMddHHmmssff");
            var filePath = $"{OutputDirectoryInfo.Name}\\{outputFileName}.mp3";

            using (var output = new FileStream(filePath, FileMode.Create)) {
                stream.CopyTo(output);
                stream.Flush();
                stream.Close();
            }

            play(filePath);
        }

        public void stop() {
            throw new NotImplementedException();
        }

        private void play(string filePath) {
            WMP.URL = filePath;
            WMP.controls.play();
        }
    }
}
