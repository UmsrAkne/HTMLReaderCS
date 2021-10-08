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

    public class TextPlayer : BindableBase, IPlayer
    {
        private ITalker talker;
        private FileInfo selectedFile;
        private List<string> texts = new List<string>();
        private int selectedTextIndex = 0;
        private int selectedFileIndex;

        private OutputFileInfo outputFileInfo;
        private SQLiteHelper sqLiteHelper = new SQLiteHelper();
        private Stopwatch stopwatch = new Stopwatch();
        private DelegateCommand playCommand;
        private DelegateCommand playFromIndexCommand;
        private DelegateCommand jumpToUnreadCommand;
        private DelegateCommand stopCommand;

        public TextPlayer(ITalker talker)
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

        public ObservableCollection<FileInfo> FileList { get; set; } = new ObservableCollection<FileInfo>();

        public AzureSSMLGen SSMLConverter { get; } = new AzureSSMLGen();

        public string PlayingPlainText { get; private set; } = string.Empty;

        public int PlayingIndex { get; set; } = 0;

        public FileInfo SelectedFile
        {
            get => selectedFile;
            set
            {
                // 選択中のコンテンツが切り替わった時点で現在の再生状況はリセットするのが妥当。
                PlayingIndex = 0;
                this.talker.stop();
                SetProperty(ref selectedFile, value);

                Texts = File.ReadAllLines(selectedFile.FullName).ToList<string>();
                CurrentFileHash = HashGenerator.getMD5Hash(File.ReadAllText(selectedFile.FullName));
            }
        }

        public List<string> Texts
        {
            get => texts;
            set => SetProperty(ref texts, value);
        }

        public int SelectedTextIndex
        {
            get => selectedTextIndex;
            set => SetProperty(ref selectedTextIndex, value);
        }

        public int SelectedFileIndex
        {
            get => selectedFileIndex;
            set => SetProperty(ref selectedFileIndex, value);
        }

        public DelegateCommand PlayCommand
        {
            get => playCommand ?? (playCommand = new DelegateCommand(
                () =>
                {

                    if (Texts.Count == 0)
                    {
                        return;
                    }

                    if (Texts.Count <= PlayingIndex)
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

                    PlayingPlainText = Texts[PlayingIndex];
                    int emptyLineCount = 0;

                    // PlayingPlainText が空文字だった場合はスキップして次の行を入力する。
                    while (string.IsNullOrEmpty(PlayingPlainText))
                    {
                        emptyLineCount++;
                        PlayingIndex++;
                        PlayingPlainText = Texts[PlayingIndex];

                        if (Texts.Count <= PlayingIndex)
                        {
                            break;
                        }
                    }

                    // 空行があった場合は、行数に応じてウェイトを挟む。
                    SSMLConverter.BeforeWait = new TimeSpan(0, 0, 0, 0, BlankLineWaitTime * emptyLineCount);
                    talker.ssmlTalk(SSMLConverter.getSSML(Texts[PlayingIndex]));

                    stopwatch.Start();
                    outputFileInfo = new OutputFileInfo();
                    outputFileInfo.HeaderText = PlayingPlainText.Substring(0, Math.Min(50, PlayingPlainText.Length));
                    outputFileInfo.OutputDateTime = DateTime.Now;
                    outputFileInfo.FileName = talker.OutputFileName;
                    outputFileInfo.Hash = CurrentFileHash;
                    outputFileInfo.Position = PlayingIndex;
                }));
        }

        public DelegateCommand PlayFromIndexCommand
        {
            get => playFromIndexCommand ?? (playFromIndexCommand = new DelegateCommand(() =>
            {
                if (SelectedTextIndex < Texts.Count)
                {
                    talker.stop();
                    PlayingIndex = SelectedTextIndex;
                    PlayCommand.Execute();
                }
            }));
        }

        public DelegateCommand JumpToUnreadCommand
        {
            get => jumpToUnreadCommand ?? (jumpToUnreadCommand = new DelegateCommand(() =>
            {
                talker.stop();
                var unreadLineNumber = sqLiteHelper.getUnreadLine(CurrentFileHash);
                PlayingIndex = unreadLineNumber;
                SelectedTextIndex = unreadLineNumber;
            }));
        }

        public DelegateCommand StopCommand
        {
            get => stopCommand ?? (stopCommand = new DelegateCommand(
            () =>
            {
                talker.stop();
            }));
        }

        private int BlankLineWaitTime { get; } = 750;

        private string CurrentFileHash { get; set; } = string.Empty;

        public void resetFiles()
        {
            StopCommand.Execute();
            FileList.Clear();
            SelectedFile = null;
        }

    }
}
