using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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

        private SSMLConverter titleTagConverter = new SSMLConverter();
        private SSMLConverter pTagConverter = new SSMLConverter();
        private SSMLConverter hTagConverter = new SSMLConverter();

        private ITalker talker;

        private int PlayingIndex { get; set; } = 0;
        public String PlayingPlainText { get; private set; } = "";

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
                    if(htmlContents.TextElements.Count <= PlayingIndex) {
                        return;
                    }

                    PlayingPlainText = htmlContents.TextElements[PlayingIndex].TextContent;

                    SSMLConverter converter;
                    switch(htmlContents.TextElements[PlayingIndex].TagName){
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
    }
}
