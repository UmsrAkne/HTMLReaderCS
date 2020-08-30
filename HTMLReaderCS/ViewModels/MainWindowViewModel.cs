using HTMLReaderCS.models;
using Prism.Mvvm;
using System.IO;
using System.Text;

namespace HTMLReaderCS.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private HTMLContents htmlContents;
        public HTMLContents HtmlContents {
            get => htmlContents;
            set => SetProperty(ref htmlContents, value);
        }

        public MainWindowViewModel()
        {

        }
    }
}
