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

        private string _title = "HTML Reader CS";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private IPlayer player;
        public IPlayer Player {
            get => player;
            set => SetProperty(ref player, value);
        }

        private IDialogService dialogService;

        public MainWindowViewModel(IDialogService _dialogService) {
            dialogService = _dialogService;
        }


        public DelegateCommand PlayNextCommand {
            #region
            get => playNextCommand ?? (playNextCommand = new DelegateCommand(() => {
                Player.StopCommand.Execute();
                Player.PlayingIndex++;
                Player.PlayCommand.Execute();
            }));
        }
        private DelegateCommand playNextCommand;
        #endregion

        public DelegateCommand ResetFileListCommand {
            #region
            get => resetFileListCommand ?? (resetFileListCommand = new DelegateCommand(() => {
                Player.resetFiles();
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
