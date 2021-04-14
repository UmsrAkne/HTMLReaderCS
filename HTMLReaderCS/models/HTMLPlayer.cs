using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private SSMLConverter titleTagConverter = new SSMLConverter();
        private SSMLConverter pTagConverter = new SSMLConverter();
        private SSMLConverter hTagConverter = new SSMLConverter();

        private ITalker talker;

        private int PlayingIndex { get; set; } = 0;
        public String PlayingPlainText { get; private set; } = "";

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

        public HTMLPlayer(ITalker talker) {
            this.talker = talker;
            this.talker.TalkEnded += (sender, e) => {
                PlayingIndex++;
                PlayCommand.Execute();
            };

            titleTagConverter.Emphasis = Emphasis.strong;

            hTagConverter.Emphasis = Emphasis.moderate;

            pTagConverter.DoReplaceNewLineToBreak = true;
            pTagConverter.Break = Break.Medium;
        }

        private DelegateCommand playCommand;
        public DelegateCommand PlayCommand {
            get => playCommand ?? (playCommand = new DelegateCommand(
                () => {
                    if(SelectedItem.TextElements.Count <= PlayingIndex) {
                        return;
                    }

                    PlayingPlainText = SelectedItem.TextElements[PlayingIndex].TextContent;

                    SSMLConverter converter;
                    switch(SelectedItem.TextElements[PlayingIndex].TagName){
                        case "TITLE":
                            converter = titleTagConverter;
                            break;
                        case "H1":
                            converter = hTagConverter;
                            break;
                        case "H2":
                            converter = hTagConverter;
                            break;
                        case "H3":
                            converter = hTagConverter;
                            break;
                        case "H4":
                            converter = hTagConverter;
                            break;
                        case "H5":
                            converter = hTagConverter;
                            break;
                        default:
                            converter = pTagConverter;
                            break;
                    }
                    converter.Text = PlayingPlainText;
                    talker.ssmlTalk(converter.getSSML());
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
