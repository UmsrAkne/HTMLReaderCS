using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models {
    public interface IPlayer {
        ObservableCollection<FileInfo> FileList { get; set; }
        FileInfo SelectedFile { get; set; }
        int SelectedFileIndex { get; set; }
        int SelectedTextIndex { get; set; }

        DelegateCommand PlayCommand { get; }
        DelegateCommand StopCommand { get; }

    }
}
