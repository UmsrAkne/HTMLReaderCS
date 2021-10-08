namespace HTMLReaderCS.ViewModels
{
    using HTMLReaderCS.Models;
    using HTMLReaderCS.Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Services.Dialogs;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "HTML Reader CS";
        private IPlayer player;
        private IDialogService dialogService;
        private DelegateCommand playNextCommand;
        private DelegateCommand resetFileListCommand;
        private DelegateCommand showHistoryWindowCommand;

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public IPlayer Player
        {
            get => player;
            set => SetProperty(ref player, value);
        }

        public DelegateCommand PlayNextCommand
        {
            get => playNextCommand ?? (playNextCommand = new DelegateCommand(() =>
            {
                Player.StopCommand.Execute();
                Player.PlayingIndex++;
                Player.PlayCommand.Execute();
            }));
        }

        public DelegateCommand ResetFileListCommand
        {
            get => resetFileListCommand ?? (resetFileListCommand = new DelegateCommand(() =>
            {
                Player.ResetFiles();
            }));
        }

        public DelegateCommand ShowHistoryWindowCommand
        {
            get => showHistoryWindowCommand ?? (showHistoryWindowCommand = new DelegateCommand(() =>
            {
                dialogService.ShowDialog(nameof(HistoryWindow), new DialogParameters(), (IDialogResult result) => { });
            }));
        }
    }
}
