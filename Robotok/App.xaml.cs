using Microsoft.Win32;
using Robotok.View;
using Robotok.ViewModel;
using RobotokModel.Model;
using RobotokModel.Model.Interfaces;
using System.ComponentModel;
using System.Windows;

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
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            //create simulation
            _simulation = new Simulation();

            // create viewModel
            _viewModel = new MainWindowViewModel(_simulation);
            _viewModel.LoadSimulation += new EventHandler((_,_) => ViewModel_LoadSimulation());
            _viewModel.SaveSimulation += new EventHandler((_,_) => ViewModel_LoadSimulation());

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

        #region ViewModel event handlers

        private void ViewModel_LoadSimulation()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Konfigurációs fájl betöltése";
            openFileDialog.Filter = "Config file|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                _simulation.Mediator.LoadSimulation(openFileDialog.FileName);
            }
        }

        private void ViewModel_SaveSimulation()
        {
            if (_simulation.Mediator is not ISimulationMediator)
                return;
            ISimulationMediator simulationMediator = (ISimulationMediator)_simulation.Mediator;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Naplófájl mentése";
            saveFileDialog.Filter = "Log file|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                simulationMediator.SaveSimulation(saveFileDialog.FileName);
            }
        }

        #endregion

    }

}
