using Robotok.View;
using Robotok.ViewModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace Robotok
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private MainWindowViewModel _viewModel = null!;
        private MainWindow _view = null!;

        #endregion

        #region Constructors

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // create viewModel
            _viewModel = new MainWindowViewModel();

            // create view
            _view = new MainWindow();
            _view.SetDataContext(_viewModel);
            //_view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();

        }

        private void View_Closing(object? sender, CancelEventArgs e)
        {
            //
        }

        #endregion
    }

}
