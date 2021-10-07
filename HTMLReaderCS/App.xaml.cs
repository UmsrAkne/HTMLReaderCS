namespace HTMLReaderCS
{
    using System.Windows;
    using HTMLReaderCS.ViewModels;
    using HTMLReaderCS.Views;
    using Prism.Ioc;
    using Prism.Modularity;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<HistoryWindow, HistoryWindowViewModel>();
        }
    }
}
