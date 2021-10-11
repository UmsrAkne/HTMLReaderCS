namespace HTMLReaderCS.Models
{
    using Prism.Mvvm;

    public class LineText : BindableBase
    {
        private bool isSelected;

        public string Text { get; set; }

        public int LineNumber { get; set; }

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public bool IsEmpty => string.IsNullOrWhiteSpace(Text);
    }
}
