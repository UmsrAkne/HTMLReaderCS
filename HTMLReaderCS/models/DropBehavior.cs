using HTMLReaderCS.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace HTMLReaderCS.Models {
    class DropBehavior : Behavior<Window> {

        protected override void OnAttached() {
            base.OnAttached();

            // ファイルをドラッグしてきて、コントロール上に乗せた際の処理
            this.AssociatedObject.PreviewDragOver += AssociatedObject_PreviewDragOver;

            // ファイルをドロップした際の処理
            this.AssociatedObject.Drop += AssociatedObject_Drop;
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e) {
            // ファイルパスの一覧の配列
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var vm = ((Window)sender).DataContext as MainWindowViewModel;

            foreach(string filePath in files) {
                using (var reader = new StreamReader(filePath)) {

                    if(Path.GetExtension(filePath) == ".txt") {
                        vm.TextPlayer.FileList.Add(new FileInfo(filePath));
                    }
                    else {
                        var text = reader.ReadToEnd();
                        var htmlContents = new models.HTMLContents(text, true);
                        htmlContents.FileName = Path.GetFileName(filePath);
                        vm.HTMLPlayer.HtmlContentsList.Add(htmlContents);
                    }
                }
            }

            if(vm.HTMLPlayer.SelectedItem == null && vm.HTMLPlayer.HtmlContentsList.Count > 0) {
                vm.HTMLPlayer.SelectedItem = vm.HTMLPlayer.HtmlContentsList[0];
            }

        }

        private void AssociatedObject_PreviewDragOver(object sender, DragEventArgs e) {
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        protected override void OnDetaching() {
            base.OnDetaching();
            this.AssociatedObject.PreviewDragOver -= AssociatedObject_PreviewDragOver;
            this.AssociatedObject.Drop -= AssociatedObject_Drop;
        }
    }
}
