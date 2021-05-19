﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models
{
    public class TextPlayer : BindableBase, IPlayer {

        public ObservableCollection<FileInfo> FileList { get; set; } = new ObservableCollection<FileInfo>();

        private ITalker talker;

        public AzureSSMLGen SSMLConverter { get; } = new AzureSSMLGen();

        private int PlayingLineNumber { get; set; } = 0;
        public String PlayingPlainText { get; private set; } = "";

        private OutputFileInfo outputFileInfo;
        private SQLiteHelper sqLiteHelper = new SQLiteHelper();
        private Stopwatch stopwatch = new Stopwatch();
        private int BlankLineWaitTime { get; } = 1000;

        public FileInfo SelectedFile {
            get => selectedFile;
            set {
                // 選択中のコンテンツが切り替わった時点で現在の再生状況はリセットするのが妥当。
                PlayingLineNumber = 0;
                this.talker.stop();
                SetProperty(ref selectedFile, value);

                Texts = File.ReadAllLines(selectedFile.FullName).ToList<string>();
            }
        }
        private FileInfo selectedFile;

        private List<string> texts = new List<string>();
        public List<string> Texts {
            get => texts;
            set => SetProperty(ref texts, value);
        }

        public int SelectedTextIndex {
            get => selectedTextIndex;
            set => SetProperty(ref selectedTextIndex, value);
        }
        private int selectedTextIndex = 0;

        public int SelectedFileIndex {
            get => selectedFileIndex;
            set => SetProperty(ref selectedFileIndex, value);
        }
        private int selectedFileIndex;

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

        public void resetFiles() {
            StopCommand.Execute();
            FileList.Clear();
            SelectedFile = null;
        }

        private DelegateCommand playCommand;
        public DelegateCommand PlayCommand {
            get => playCommand ?? (playCommand = new DelegateCommand(
                () => {

                    if(Texts.Count == 0) {
                        return;
                    }

                    if(Texts.Count <= PlayingLineNumber) {
                        if(SelectedFileIndex < FileList.Count -1) {
                            SelectedFileIndex++;
                            SelectedFile = FileList[SelectedFileIndex];
                        }
                        else {
                            return; // 次の HTML ファイルが存在しない場合は処理を中止
                        }
                    }

                    PlayingPlainText = Texts[PlayingLineNumber];
                    int emptyLineCount = 0;

                    // PlayingPlainText が空文字だった場合はスキップして次の行を入力する。
                    while (String.IsNullOrEmpty(PlayingPlainText)) {
                        emptyLineCount++;
                        PlayingLineNumber++;
                        PlayingPlainText = Texts[PlayingLineNumber];

                        if(Texts.Count <= PlayingLineNumber) {
                            break;
                        }
                    }

                    // 空行があった場合は、行数に応じてウェイトを挟む。
                    SSMLConverter.BeforeWait = new TimeSpan(0, 0, 0, 0, BlankLineWaitTime * emptyLineCount);
                    talker.ssmlTalk(SSMLConverter.getSSML(Texts[PlayingLineNumber]));

                    stopwatch.Start();
                    outputFileInfo = new OutputFileInfo();
                    outputFileInfo.HeaderText = PlayingPlainText.Substring(0, Math.Min(50,PlayingPlainText.Length));
                    outputFileInfo.OutputDateTime = DateTime.Now;
                    outputFileInfo.FileName = talker.OutputFileName;
                }
            ));
        }

        public DelegateCommand PlayFromIndexCommand {
            #region
            get => playFromIndexCommand ?? (playFromIndexCommand = new DelegateCommand(() => {
                if(SelectedTextIndex < Texts.Count) {
                    talker.stop();
                    PlayingLineNumber = SelectedTextIndex;
                    PlayCommand.Execute();
                }
            }));
        }
        private DelegateCommand playFromIndexCommand;
        #endregion

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
