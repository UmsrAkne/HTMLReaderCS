namespace HTMLReaderCS.Models
{
    public class LineText
    {
        public string Text { get; set; }

        public int LineNumber { get; set; }

        public bool IsSelected { get; set; }

        public bool IsEmpty => string.IsNullOrWhiteSpace(Text);
    }
}
