using HTMLReaderCS.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCSTests.models
{
    class DummyTalker : ITalker
    {
        public event EventHandler TalkEnded;

        public String PlayingText { get; set; }

        public string OutputFileName { get; }

        public void ssmlTalk(string ssmlText)
        {
            PlayingText = ssmlText;
        }

        public void dispatchTalkEnded()
        {
            TalkEnded(this, new EventArgs());
        }

        public void stop()
        {
        }
    }
}
