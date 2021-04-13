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
using System.Windows.Threading;

namespace HTMLReaderCS.models {
    public class PollyPlayer : ITalker {

        public SSMLConverter SsmlConverter { get; set; } = new SSMLConverter();

        private DirectoryInfo OutputDirectoryInfo { get; set; } = new DirectoryInfo("outputs");

        private AmazonPollyClient PollyClient { get; set; }

        private WindowsMediaPlayer WMP { get; set; } = new WindowsMediaPlayer();

        private DispatcherTimer timer;

        public event EventHandler TalkEnded;

        public PollyPlayer() {
            var accKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var secKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            PollyClient = new AmazonPollyClient(accKey, secKey, Amazon.RegionEndpoint.APNortheast1);

            if (!OutputDirectoryInfo.Exists) {
                OutputDirectoryInfo.Create();
            }

            WMP.PlayStateChange += WMP_PlayStateChange;

            /*
             * WMPによる音声の再生が終了した際、この timer をスタートさせる。
             * そして、更に１秒後、このタイマーのイベントハンドラによって再生終了イベントを送出する。
             *
             * この実装の理由 : 
             * WMP の再生終了から直接イベントを送出した場合、次の音声が再生されないため……。
             * 本当に原因がわからないけど、時間差でイベントを飛ばせば正常に動くようなので、やむを得ずこのような実装にする。
             * どうしてか本当にわからない……。
            */
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += (sender, e) => {
                TalkEnded(this,new EventArgs());

                // timer は一回開始するごとに、一回だけ動作すればいいので、このメソッドが実行した時点で止める。
                timer.Stop();
            };
        }

        private void WMP_PlayStateChange(int NewState) {
            if((int)WMP.playState == (int)WMPPlayState.wmppsMediaEnded) {
                timer.Start();
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
