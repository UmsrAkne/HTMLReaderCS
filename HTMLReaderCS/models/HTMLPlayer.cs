namespace HTMLReaderCS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Prism.Commands;
    using Prism.Mvvm;

    public class HTMLPlayer : BindableBase, IPlayer
    {

        private FileInfo selectedFile;
        private ITalker talker;

        private OutputFileInfo outputFileInfo;
        private SQLiteHelper sqLiteHelper = new SQLiteHelper();
        private Stopwatch stopwatch = new Stopwatch();
        private DelegateCommand playCommand;
        private DelegateCommand playFromIndexCommand;
        private DelegateCommand jumpToUnreadCommand;
        private DelegateCommand stopCommand;
        private int selectedFileIndex = 0;
        private int selectedTextIndex;
        private List<string> texts = new List<string>();

        public HTMLPlayer(ITalker talker)
        {
            this.talker = talker;
            this.talker.TalkEnded += (sender, e) =>
            {

                stopwatch.Stop();
                outputFileInfo.LengthSec = (int)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Reset();

                sqLiteHelper.insert(outputFileInfo);

                PlayingIndex++;
                PlayCommand.Execute();
            };

            SSMLConverter.Rate = 85;
        }

        public FileInfo SelectedFile
        {
            get => selectedFile;
            set
            {
                currentHtmlContents = new HTMLContents(File.ReadAllText(value.FullName));

                // 選択中のコンテンツが切り替わった時点で現在の再生状況はリセットするのが妥当。
                PlayingIndex = 0;
                this.talker.stop();

                SetProperty(ref selectedFile, value);
            }
        }

        public ObservableCollection<FileInfo> FileList { get; set; } = new ObservableCollection<FileInfo>();

        public AzureSSMLGen SSMLConverter { get; } = new AzureSSMLGen();

        public int PlayingIndex { get; set; } = 0;
        public string PlayingPlainText { get; private set; } = string.Empty;

        public int SelectedFileIndex
        {
            get => selectedFileIndex;
            set => SetProperty(ref selectedFileIndex, value);
        }

        public int SelectedTextIndex
        {
            get => selectedTextIndex;
            set => SetProperty(ref selectedTextIndex, value);
        }

        public List<string> Texts
        {
            get => texts;
            set => SetProperty(ref texts, value);
        }

        public DelegateCommand PlayCommand
        {
            get => playCommand ?? (playCommand = new DelegateCommand(() =>
                {

                    if (currentHtmlContents.TextElements.Count <= PlayingIndex)
                    {
                        if (SelectedFileIndex < FileList.Count - 1)
                        {
                            SelectedFileIndex++;
                            SelectedFile = FileList[SelectedFileIndex];
                        }
                        else
                        {
                            return; // 次の HTML ファイルが存在しない場合は処理を中止
                        }
                    }

                    PlayingPlainText = currentHtmlContents.TextElements[PlayingIndex].TextContent;

                    talker.ssmlTalk(SSMLConverter.getSSML(PlayingPlainText));

                    stopwatch.Start();
                    outputFileInfo = new OutputFileInfo();
                    outputFileInfo.HeaderText = PlayingPlainText.Substring(0, Math.Min(50, PlayingPlainText.Length));
                    outputFileInfo.OutputDateTime = DateTime.Now;
                    outputFileInfo.TagName = currentHtmlContents.TextElements[PlayingIndex].TagName;
                    outputFileInfo.FileName = talker.OutputFileName;
                    outputFileInfo.HtmlFileName = currentHtmlContents.FileName;
                }));
        }

        public DelegateCommand PlayFromIndexCommand
        {
            get => playFromIndexCommand ?? (playFromIndexCommand = new DelegateCommand(() =>
            {
            }));
        }

        public DelegateCommand JumpToUnreadCommand
        {
            get => jumpToUnreadCommand ?? (jumpToUnreadCommand = new DelegateCommand(() =>
            {
            }));
        }

        public DelegateCommand StopCommand
        {
            get => stopCommand ?? (stopCommand = new DelegateCommand(() =>
            {
                talker.stop();
            }));
        }

        private HTMLContents currentHtmlContents { get; set; }

        public void resetFiles()
        {
            StopCommand.Execute();
            FileList.Clear();
            SelectedFile = null;
        }
    }
}
