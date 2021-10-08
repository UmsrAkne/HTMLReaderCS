namespace HTMLReaderCSTests.models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HTMLReaderCS.Models;

    public class DummyTalker : ITalker
    {
        public event EventHandler TalkEnded;

        public string PlayingText { get; set; }

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
