using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models {
    class PollyPlayer : ITalker {
        public event EventHandler TalkEnded;

        public void ssmlTalk(string ssmlText) {
            throw new NotImplementedException();
        }

        public void stop() {
            throw new NotImplementedException();
        }

    }
}
