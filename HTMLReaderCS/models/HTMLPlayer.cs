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
    public class HTMLPlayer : BindableBase{

        private HTMLContents htmlContents; 
        public HTMLContents HtmlContents {
            get => htmlContents;
            set => SetProperty(ref htmlContents, value);
        }

        public ObservableCollection<HTMLContents> HtmlContentsList { get; set; } = new ObservableCollection<HTMLContents>();

        private ITalker talker;

        public AzureSSMLGen SSMLConverter { get; } = new AzureSSMLGen();

        private int PlayingIndex { get; set; } = 0;
        public String PlayingPlainText { get; private set; } = "";

        private OutputFileInfo outputFileInfo;
        private SQLiteHelper sqLiteHelper = new SQLiteHelper();
        private Stopwatch stopwatch = new Stopwatch();

        public HTMLContents SelectedItem { 
            get => selectedItem;
            set {
                // 選択中のコンテンツが切り替わった時点で現在の再生状況はリセットするのが妥当。
                PlayingIndex = 0;
                this.talker.stop();

                SetProperty(ref selectedItem, value);
            }
        }
        private HTMLContents selectedItem;

        public int SelectedContentIndex {
            get => selectedContentIndex;
            set => SetProperty(ref selectedContentIndex, value);
        }
        private int selectedContentIndex = 0;

        public HTMLPlayer(ITalker talker) {
            this.talker = talker;
            this.talker.TalkEnded += (sender, e) => {

                stopwatch.Stop();
                outputFileInfo.LengthSec = (int)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Reset();

                sqLiteHelper.insert(outputFileInfo);

                PlayingIndex++;
                PlayCommand.Execute();
            };

            SSMLConverter.Rate = 85;
        }

        public void resetContents() {
            StopCommand.Execute();
            HtmlContentsList.Clear();
            SelectedItem = null;
        }

        private DelegateCommand playCommand;
        public DelegateCommand PlayCommand {
            get => playCommand ?? (playCommand = new DelegateCommand(
                () => {

                    if(SelectedItem.TextElements.Count <= PlayingIndex) {
                        if(SelectedContentIndex < HtmlContentsList.Count -1) {
                            SelectedContentIndex++;
                            SelectedItem = HtmlContentsList[SelectedContentIndex];
                        }
                        else {
                            return; // 次の HTML ファイルが存在しない場合は処理を中止
                        }
                    }

                    PlayingPlainText = SelectedItem.TextElements[PlayingIndex].TextContent;

                    talker.ssmlTalk(SSMLConverter.getSSML(PlayingPlainText));

                    stopwatch.Start();
                    outputFileInfo = new OutputFileInfo();
                    outputFileInfo.HeaderText = PlayingPlainText.Substring(0, Math.Min(50,PlayingPlainText.Length));
                    outputFileInfo.OutputDateTime = DateTime.Now;
                    outputFileInfo.TagName = SelectedItem.TextElements[PlayingIndex].TagName;
                    outputFileInfo.FileName = talker.OutputFileName;
                    outputFileInfo.HtmlFileName = SelectedItem.FileName;
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
