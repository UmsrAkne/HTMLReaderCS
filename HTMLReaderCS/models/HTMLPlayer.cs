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

        private SSMLConverter ssmlConverter;
        private ITalker talker;

        public HTMLPlayer(ITalker talker) {
            this.talker = talker;
            this.talker.TalkEnded += (sender, e) => {

            };
        }
    }
}
