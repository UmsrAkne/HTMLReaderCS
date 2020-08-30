using AngleSharp.Html.Dom;
using AngleSharp.Dom;
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

        private List<String> targetTags = new List<String>(new String[]{ 
            "P","TITLE", "H1","H2","H3","H4","H5"});

        /// <summary>
        /// TextElementsプロパティを呼び出した際、このリストに含まれるタグを抜き出し、リストを作成します。
        /// </summary>
        public List<String> TargetTags {
            get => targetTags;
            set {
                targetTags = value;

                // textElements を再作成。"_" はプロパティ呼び出しのために定義して使用しない
                textElements = null;
                var _ = TextElements;
            }
        }

        public IHtmlAllCollection getAllElement() {
            return htmlDocument.All;
        }

        private List<IElement> textElements;

        /// <summary>
        /// 内部で保持するHTMLDocument から、テキストを含む要素を抜き出し、リストとして取得します。
        /// </summary>
        public List<IElement> TextElements {
            get {
                if(textElements == null) {
                    textElements = new List<IElement>();
                    htmlDocument.All.ToList().ForEach((e) => {
                        if(TargetTags.Any(t => t == e.TagName)) {
                            if (!String.IsNullOrEmpty(e.TextContent)) {
                                textElements.Add(e);
                            }
                        }
                    });
                }
                return textElements;
            }
        }

    }
}
