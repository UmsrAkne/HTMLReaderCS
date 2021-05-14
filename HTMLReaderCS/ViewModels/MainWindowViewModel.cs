using HTMLReaderCS.models;
using HTMLReaderCS.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
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

        private IPlayer textPlayer;
        public IPlayer TextPlayer {
            get => textPlayer;
            set => SetProperty(ref textPlayer, value);
        }

        private IDialogService dialogService;

        public MainWindowViewModel(IDialogService _dialogService) {
            dialogService = _dialogService;
            HTMLPlayer = new HTMLPlayer(new AzureTalker());
            TextPlayer = new TextPlayer(new AzureTalker());
        }

        public DelegateCommand ResetFileListCommand {
            #region
            get => resetFileListCommand ?? (resetFileListCommand = new DelegateCommand(() => {
                HTMLPlayer.resetContents();
            }));
        }
        private DelegateCommand resetFileListCommand;
        #endregion


        public DelegateCommand ShowHistoryWindowCommand {
            #region
            get => showHistoryWindowCommand ?? (showHistoryWindowCommand = new DelegateCommand(() => {
                dialogService.ShowDialog(nameof(HistoryWindow), new DialogParameters(), (IDialogResult result) => {
                });
            }));
        }
        private DelegateCommand showHistoryWindowCommand;
        #endregion

    }
}
