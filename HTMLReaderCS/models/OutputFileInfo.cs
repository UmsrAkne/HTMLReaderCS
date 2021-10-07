using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models
{
    public class OutputFileInfo
    {

        public int LengthSec { get; set; }
        public string FileName { get; set; }
        public string HtmlFileName { get; set; }
        public string HeaderText { get; set; }
        public string TagName { get; set; }
        public DateTime OutputDateTime { get; set; }

        public string Hash { get; set; }
        public int Position { get; set; }

        /// <summary>
        /// このオブジェクトを生成した時点でファイルが存在するかどうかを示します。
        /// </summary>
        public bool Exists { get; set; }

    }
}
