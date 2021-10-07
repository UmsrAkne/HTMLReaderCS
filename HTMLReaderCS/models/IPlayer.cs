namespace HTMLReaderCS.models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Prism.Commands;

    public interface IPlayer
    {
        ObservableCollection<FileInfo> FileList { get; set; }
        FileInfo SelectedFile { get; set; }
        int SelectedFileIndex { get; set; }
        int SelectedTextIndex { get; set; }
        int PlayingIndex { get; set; }

        List<string> Texts { get; }

        DelegateCommand PlayCommand { get; }
        DelegateCommand PlayFromIndexCommand { get; }
        DelegateCommand StopCommand { get; }
        DelegateCommand JumpToUnreadCommand { get; }

        void resetFiles();

    }
}
