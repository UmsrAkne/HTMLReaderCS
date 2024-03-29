﻿namespace HTMLReaderCS.Models
{
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using HTMLReaderCS.Models;
    using HTMLReaderCS.ViewModels;

    public class DropBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            // ファイルをドラッグしてきて、コントロール上に乗せた際の処理
            this.AssociatedObject.PreviewDragOver += AssociatedObject_PreviewDragOver;

            // ファイルをドロップした際の処理
            this.AssociatedObject.Drop += AssociatedObject_Drop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewDragOver -= AssociatedObject_PreviewDragOver;
            this.AssociatedObject.Drop -= AssociatedObject_Drop;
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            // ファイルパスの一覧の配列
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var vm = ((Window)sender).DataContext as MainWindowViewModel;

            if (vm.Player == null)
            {
                vm.Player =
                (Path.GetExtension(files[0]) == "html")
                    ? (IPlayer)new HTMLPlayer(new AzureTalker())
                    : (IPlayer)new TextPlayer(new AzureTalker());
            }

            foreach (string filePath in files)
            {
                using (var reader = new StreamReader(filePath))
                {
                    vm.Player.FileList.Add(new FileInfo(filePath));
                }
            }

            if (vm.Player.SelectedFile == null && vm.Player.FileList.Count > 0)
            {
                vm.Player.SelectedFile = vm.Player.FileList[0];
            }
        }

        private void AssociatedObject_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }
    }
}
