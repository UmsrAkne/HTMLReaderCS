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

        private SSMLConverter ssmlConverter = new SSMLConverter();
        private ITalker talker;

        private int PlayingIndex { get; set; } = 0;
        public String PlayingPlainText { get; private set; } = "";

        public HTMLPlayer(ITalker talker) {
            this.talker = talker;
            this.talker.TalkEnded += (sender, e) => {
                PlayingIndex++;
                PlayCommand.Execute();
            };
        }

        private DelegateCommand playCommand;
        public DelegateCommand PlayCommand {
            get => playCommand ?? (playCommand = new DelegateCommand(
                () => {
                    if(htmlContents.TextElements.Count <= PlayingIndex) {
                        return;
                    }

                    PlayingPlainText = htmlContents.TextElements[PlayingIndex].TextContent;
                    ssmlConverter.Text = PlayingPlainText;
                    talker.ssmlTalk(ssmlConverter.getSSML());
                }
            ));
        }
    }
}
