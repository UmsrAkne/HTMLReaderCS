using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models
{
    public class TextPlayer : BindableBase{

        public ObservableCollection<string> TextFiles { get; set; } = new ObservableCollection<string>();

        private ITalker talker;

        public AzureSSMLGen SSMLConverter { get; } = new AzureSSMLGen();

        private int PlayingLineNumber { get; set; } = 0;
        public String PlayingPlainText { get; private set; } = "";

        private OutputFileInfo outputFileInfo;
        private SQLiteHelper sqLiteHelper = new SQLiteHelper();
        private Stopwatch stopwatch = new Stopwatch();

        public string SelectedFile { 
            get => selectedFile;
            set {
                // 選択中のコンテンツが切り替わった時点で現在の再生状況はリセットするのが妥当。
                PlayingLineNumber = 0;
                this.talker.stop();

                SetProperty(ref selectedFile, value);
            }
        }
        private string selectedFile;

        public int SelectedFileIndex {
            get => selectedFileIndex;
            set => SetProperty(ref selectedFileIndex, value);
        }
        private int selectedFileIndex = 0;

        public TextPlayer(ITalker talker) {
            this.talker = talker;
            this.talker.TalkEnded += (sender, e) => {

                stopwatch.Stop();
                outputFileInfo.LengthSec = (int)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Reset();

                sqLiteHelper.insert(outputFileInfo);

                PlayingLineNumber++;
                PlayCommand.Execute();
            };

            SSMLConverter.Rate = 85;
        }

        public void resetContents() {
            StopCommand.Execute();
            TextFiles.Clear();
            SelectedFile = null;
        }

        private DelegateCommand playCommand;
        public DelegateCommand PlayCommand {
            get => playCommand ?? (playCommand = new DelegateCommand(
                () => {

                    //if(SelectedFile..Count <= PlayingLineNumber) {
                    //    if(SelectedFileIndex < TextFiles.Count -1) {
                    //        SelectedFileIndex++;
                    //        SelectedFile = TextFiles[SelectedFileIndex];
                    //    }
                    //    else {
                    //        return; // 次の HTML ファイルが存在しない場合は処理を中止
                    //    }
                    //}

                    //PlayingPlainText = SelectedFile[PlayingLineNumber];

                    //talker.ssmlTalk(SSMLConverter.getSSML(SelectedFile[PlayingLineNumber]));

                    //stopwatch.Start();
                    //outputFileInfo = new OutputFileInfo();
                    //outputFileInfo.HeaderText = PlayingPlainText.Substring(0, Math.Min(50,PlayingPlainText.Length));
                    //outputFileInfo.OutputDateTime = DateTime.Now;
                    //outputFileInfo.TagName = SelectedItem.TextElements[PlayingLineNumber].TagName;
                    //outputFileInfo.FileName = talker.OutputFileName;
                    //outputFileInfo.HtmlFileName = SelectedItem.FileName;
                }
            ));
        }

        private DelegateCommand stopCommand;
        public DelegateCommand StopCommand {
            get => stopCommand ?? (stopCommand = new DelegateCommand(
                () => {
                    talker.stop();
                }
            ));
        }
    }
}
