namespace HTMLReaderCS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// テキストを読み上げる機能を持つクラスが実装するインターフェースです
    /// </summary>
    public interface ITalker
    {
        /// <summary>
        /// 読み上げの終了時に送出されるイベントです
        /// </summary>
        event EventHandler TalkEnded;

        string OutputFileName { get; }

        /// <summary>
        /// ssmlのテキストを読み上げる機能です。
        /// </summary>
        void SSMLTalk(string ssmlText);

        void Stop();
    }
}
