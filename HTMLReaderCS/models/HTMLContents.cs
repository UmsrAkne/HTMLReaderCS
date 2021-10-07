namespace HTMLReaderCS.models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using AngleSharp.Html.Parser;

    public class HTMLContents
    {
        private List<IElement> textElements;
        private IHtmlDocument htmlDocument;
        private List<string> targetTags = new List<string>(new string[] { "P", "TITLE", "H1", "H2", "H3", "H4", "H5" });

        public HTMLContents(string htmlText)
        {
            var parser = new HtmlParser();
            htmlDocument = parser.ParseDocument(htmlText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlText"></param>
        /// <param name="replaceRubyTag">true を入力した場合、ルビタグを漢字のみに置き換えた後にHTMLをパースします。</param>
        public HTMLContents(string htmlText, bool doReplaceRubyTag)
        {
            if (doReplaceRubyTag)
            {
                htmlText = replaceRubyTag(htmlText);
            }

            var parser = new HtmlParser();
            htmlDocument = parser.ParseDocument(htmlText);
        }

        public string FileName { get; set; } = "default";

        /// <summary>
        /// TextElementsプロパティを呼び出した際、このリストに含まれるタグを抜き出し、リストを作成します。
        /// </summary>
        public List<string> TargetTags
        {
            get => targetTags;
            set
            {
                targetTags = value;

                // textElements を再作成。"_" はプロパティ呼び出しのために定義して使用しない
                textElements = null;
                var _ = TextElements;
            }
        }

        /// <summary>
        /// 内部で保持するHTMLDocument から、テキストを含む要素を抜き出し、リストとして取得します。
        /// </summary>
        public List<IElement> TextElements
        {
            get
            {
                if (textElements == null)
                {
                    textElements = new List<IElement>();
                    htmlDocument.All.ToList().ForEach((e) =>
                    {
                        if (TargetTags.Any(t => t == e.TagName))
                        {
                            if (!string.IsNullOrEmpty(e.TextContent))
                            {
                                textElements.Add(e);
                            }
                        }
                    });
                }
                return textElements;
            }
        }

        public IHtmlAllCollection getAllElement()
        {
            return htmlDocument.All;
        }

        private string replaceRubyTag(string target)
        {
            target = Regex.Replace(target, " |　", string.Empty);
            target = Regex.Replace(target, "<rt>(.+?)</rt>", string.Empty);
            target = Regex.Replace(target, "<rp>(.+?)</rp>", string.Empty);
            return Regex.Replace(target, "<ruby>(.+?)</ruby>", "$1");
        }
    }
}
