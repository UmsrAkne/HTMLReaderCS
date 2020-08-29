using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models {
    class HTMLContents {

        private IHtmlDocument htmlDocument;

        public HTMLContents(String htmlText) {
            var parser = new HtmlParser();
            htmlDocument = parser.ParseDocument(htmlText);
        }
    }
}
