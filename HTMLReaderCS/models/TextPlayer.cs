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
        private List<LineText> texts = new List<LineText>();
        private int selectedTextIndex = 0;
        private int selectedFileIndex;

        private OutputFileInfo outputFileInfo;
        private SQLiteHelper sqliteHelper = new SQLiteHelper();
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

                sqliteHelper.Insert(outputFileInfo);
                Texts[PlayingIndex].IsSelected = false;

                PlayingIndex++;
                PlayCommand.Execute();
            };
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
                this.talker.Stop();
                SetProperty(ref selectedFile, value);

                Texts = File.ReadAllLines(selectedFile.FullName).ToList().Select(s => new LineText() { Text = s }).ToList();
                Enumerable.Range(0, Texts.Count).ToList().ForEach(i => Texts[i].LineNumber = i + 1);
                CurrentFileHash = HashGenerator.GetMD5Hash(File.ReadAllText(selectedFile.FullName));
            }
        }

        public List<LineText> Texts
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

                    PlayingPlainText = Texts[PlayingIndex].Text;
                    int emptyLineCount = 0;

                    // PlayingPlainText が空文字だった場合はスキップして次の行を入力する。
                    while (string.IsNullOrEmpty(PlayingPlainText))
                    {
                        emptyLineCount++;
                        PlayingIndex++;
                        PlayingPlainText = Texts[PlayingIndex].Text;

                        if (Texts.Count <= PlayingIndex)
                        {
                            break;
                        }
                    }

                    // 空行があった場合は、行数に応じてウェイトを挟む。
                    SSMLConverter.BeforeWait = new TimeSpan(0, 0, 0, 0, BlankLineWaitTime * emptyLineCount);
                    talker.SSMLTalk(SSMLConverter.GetSSML(Texts[PlayingIndex].Text));
                    Texts[PlayingIndex].IsSelected = true;

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
                    talker.Stop();
                    PlayingIndex = SelectedTextIndex;
                    PlayCommand.Execute();
                }
            }));
        }

        public DelegateCommand JumpToUnreadCommand
        {
            get => jumpToUnreadCommand ?? (jumpToUnreadCommand = new DelegateCommand(() =>
            {
                talker.Stop();
                var unreadLineNumber = sqliteHelper.GetUnreadLine(CurrentFileHash);
                PlayingIndex = unreadLineNumber;
                SelectedTextIndex = unreadLineNumber;
            }));
        }

        public DelegateCommand StopCommand
        {
            get => stopCommand ?? (stopCommand = new DelegateCommand(
            () =>
            {
                talker.Stop();
            }));
        }

        private int BlankLineWaitTime { get; } = 750;

        private string CurrentFileHash { get; set; } = string.Empty;

        public void ResetFiles()
        {
            StopCommand.Execute();
            FileList.Clear();
            SelectedFile = null;
        }
    }
}
