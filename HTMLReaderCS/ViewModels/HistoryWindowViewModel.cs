﻿using HTMLReaderCS.models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.ViewModels {
    class HistoryWindowViewModel : BindableBase, IDialogAware {
        public string Title => "履歴";

        public event Action<IDialogResult> RequestClose;

        private SQLiteHelper SQLiteHelper { get; set; }

        public List<OutputFileInfo> OutputHistory { get => outputHistory; set => SetProperty(ref outputHistory, value); }
        private List<OutputFileInfo> outputHistory;

        public OutputFileInfo SelectedItem { get => selectedItem; set => SetProperty(ref selectedItem, value); }
        private OutputFileInfo selectedItem;


        public bool CanCloseDialog() => true;

        public void OnDialogClosed() {
        }

        public void OnDialogOpened(IDialogParameters parameters) {
            SQLiteHelper = new SQLiteHelper();
            OutputHistory = SQLiteHelper.getHistories();
        }

        public DelegateCommand PlayFileCommand {
            #region
            get => playFileCommand ?? (playFileCommand = new DelegateCommand(() => {
                if(SelectedItem != null) {
                    System.Diagnostics.Debug.WriteLine(SelectedItem);
                }
            }));
        }
        private DelegateCommand playFileCommand;
        #endregion

        public DelegateCommand CloseWindowCommand {
            #region
            get => closeWindowCommand ?? (closeWindowCommand = new DelegateCommand(() => {
                RequestClose?.Invoke(new DialogResult());
            }));
        }
        private DelegateCommand closeWindowCommand;
        #endregion

    }
}
