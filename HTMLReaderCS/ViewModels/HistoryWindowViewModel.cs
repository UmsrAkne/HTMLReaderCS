namespace HTMLReaderCS.ViewModels
{
    using System;
    using System.Collections.Generic;
    using HTMLReaderCS.Models;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Services.Dialogs;
    using WMPLib;

    public class HistoryWindowViewModel : BindableBase, IDialogAware
    {
        private List<OutputFileInfo> outputHistory;
        private OutputFileInfo selectedItem;

        private DelegateCommand playFileCommand;
        private DelegateCommand stopSoundCommand;
        private DelegateCommand closeWindowCommand;

        public event Action<IDialogResult> RequestClose;

        public string Title => "履歴";

        public List<OutputFileInfo> OutputHistory { get => outputHistory; set => SetProperty(ref outputHistory, value); }

        public OutputFileInfo SelectedItem { get => selectedItem; set => SetProperty(ref selectedItem, value); }

        public DelegateCommand PlayFileCommand
        {
            get => playFileCommand ?? (playFileCommand = new DelegateCommand(() =>
            {
                if (SelectedItem != null && SelectedItem.Exists)
                {
                    WMP.URL = $"{Properties.Settings.Default.OutputDirectoryName}\\{SelectedItem.FileName}";
                    WMP.controls.play();
                }
            }));
        }

        public DelegateCommand StopSoundCommand
        {
            get => stopSoundCommand ?? (stopSoundCommand = new DelegateCommand(() =>
            {
                WMP.controls.stop();
            }));
        }

        public DelegateCommand CloseWindowCommand
        {
            get => closeWindowCommand ?? (closeWindowCommand = new DelegateCommand(() =>
            {
                RequestClose?.Invoke(new DialogResult());
            }));
        }

        private SQLiteHelper SQLiteHelper { get; set; }

        private WindowsMediaPlayer WMP { get; } = new WindowsMediaPlayer();

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            SQLiteHelper = new SQLiteHelper();
            OutputHistory = SQLiteHelper.getHistories();
        }

        public bool CanCloseDialog() => true;
    }
}
