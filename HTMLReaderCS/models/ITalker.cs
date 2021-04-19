using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models
{

    /// <summary>
    /// テキストを読み上げる機能を持つクラスが実装するインターフェースです
    /// </summary>
    public interface ITalker {

        /// <summary>
        /// ssmlのテキストを読み上げる機能です。
        /// </summary>
        void ssmlTalk(String ssmlText);

        /// <summary>
        /// 読み上げの終了時に送出されるイベントです
        /// </summary>
        event EventHandler TalkEnded;

        void stop();

        string OutputFileName { get; }
    }
}
