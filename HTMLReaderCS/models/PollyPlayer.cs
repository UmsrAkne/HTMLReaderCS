﻿namespace HTMLReaderCS.Models
{
    using System;
    using System.IO;
    using System.Windows.Threading;
    using Amazon.Polly;
    using Amazon.Polly.Model;
    using WMPLib;

    public class PollyPlayer : ITalker
    {
        private DispatcherTimer timer;

        public PollyPlayer()
        {
            var accKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var secKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            PollyClient = new AmazonPollyClient(accKey, secKey, Amazon.RegionEndpoint.APNortheast1);

            if (!OutputDirectoryInfo.Exists)
            {
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
            timer.Tick += (sender, e) =>
            {
                TalkEnded(this, new EventArgs());

                // timer は一回開始するごとに、一回だけ動作すればいいので、このメソッドが実行した時点で止める。
                timer.Stop();
            };
        }

        public event EventHandler TalkEnded;

        public SSMLConverter SsmlConverter { get; set; } = new SSMLConverter();

        /// <summary>
        /// 最後に読み上げた音声ファイル名を取得します。
        /// </summary>
        public string OutputFileName { get; private set; }

        private DirectoryInfo OutputDirectoryInfo { get; set; } = new DirectoryInfo(Properties.Settings.Default.OutputDirectoryName);

        private AmazonPollyClient PollyClient { get; set; }

        private WindowsMediaPlayer WMP { get; set; } = new WindowsMediaPlayer();

        public void SSMLTalk(string ssmlText)
        {
            var req = new SynthesizeSpeechRequest();
            req.VoiceId = VoiceId.Takumi;
            req.OutputFormat = OutputFormat.Mp3;
            req.TextType = TextType.Ssml;
            req.Text = ssmlText;

            var response = PollyClient.SynthesizeSpeech(req);

            var stream = response.AudioStream;

            OutputFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssff")}.mp3";
            var filePath = $"{OutputDirectoryInfo.Name}\\{OutputFileName}";

            using (var output = new FileStream(filePath, FileMode.Create))
            {
                stream.CopyTo(output);
                stream.Flush();
                stream.Close();
            }

            Play(filePath);
        }

        public void Stop()
        {
            WMP.controls.stop();
        }

        private void WMP_PlayStateChange(int newState)
        {
            if ((int)WMP.playState == (int)WMPPlayState.wmppsMediaEnded)
            {
                timer.Start();
            }
        }

        private void Play(string filePath)
        {
            WMP.URL = filePath;
            WMP.controls.play();
        }
    }
}
