using HTMLReaderCS.models;
using Prism.Commands;
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

        private HTMLPlayer htmlPlayer;
        public HTMLPlayer HTMLPlayer {
            get => htmlPlayer;
            set => SetProperty(ref htmlPlayer, value);
        }

        public MainWindowViewModel() {
            HTMLPlayer = new HTMLPlayer(new PollyPlayer());
        }
    }
}
