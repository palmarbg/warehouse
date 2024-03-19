using Robotok.View;
using Robotok.ViewModel;
using RobotokModel.Model;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using RobotokModel.Model.Extensions;

namespace Robotok
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private Simulation _simulation = null!;
        private MainWindowViewModel _viewModel = null!;
        private MainWindow _view = null!;

        #endregion

        #region Constructors

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);

            Debug.WriteLine((new Position() { X = 1, Y = 2 }).PoistionInDirection(Direction.Right));
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            //create simulation
            _simulation = new Simulation();

            // create viewModel
            _viewModel = new MainWindowViewModel(_simulation);

            // create view
            _view = new MainWindow();
            _view.Show();
            _view.SetDataContext(_viewModel);
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz

        }

        private void View_Closing(object? sender, CancelEventArgs e)
        {
            //
        }

        #endregion
    }

}
