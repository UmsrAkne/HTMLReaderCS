using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.ViewModels {
    class HistoryWindowViewModel : IDialogAware {
        public string Title => "履歴";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() {
        }

        public void OnDialogOpened(IDialogParameters parameters) {
        }

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
