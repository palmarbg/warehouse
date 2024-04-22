using Microsoft.Win32;
using Model;
using Model.Interfaces;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using View.Windows;
using ViewModel.ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private IServiceLocator _serviceLocator = null!;
        private ISimulation _simulation = null!;
        private MainWindowViewModel _viewModel = null!;
        private MainWindow _view = null!;

        #endregion

        #region Constructors

        public App()
        {
            try
            {
                Startup += new StartupEventHandler(App_Startup);
            }
            catch
            {
                Debug.WriteLine("Ezen a ponton lezárhatod a géped és elgondolkodhatsz az életeden mit csinálsz");
                throw new Exception();
            }
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            //create service locator
            _serviceLocator = new ServiceLocator();

            //create simulation
            _simulation = new Simulation(_serviceLocator);

            // create viewModel
            _viewModel = new MainWindowViewModel(_simulation);
            _viewModel.LoadSimulation += new EventHandler((_, _) => ViewModel_LoadSimulation());
            _viewModel.LoadReplay += new EventHandler((_, _) => ViewModel_LoadReplay());
            _viewModel.SaveSimulation += new EventHandler((_, _) => ViewModel_SaveSimulation());
            _viewModel.OpenReplaySettings += new EventHandler((_, _) => ViewModel_OpenReplaySettings());

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
                _simulation.LoadConfig(openFileDialog.FileName);
            }
        }

        private void ViewModel_LoadReplay()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Naplófájl betöltése";
            openFileDialog.Filter = "Log file|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                _simulation.LoadLog(openFileDialog.FileName);
                //_simulation.(openFileDialog.FileName);
            }
        }

        private void ViewModel_SaveSimulation()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Naplófájl mentése";
            saveFileDialog.Filter = "Log file|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                _simulation.SaveLog(saveFileDialog.FileName);
            }
        }

        private void ViewModel_OpenReplaySettings()
        {
            if (_simulation.Mediator is not IReplayMediator)
                return;

            var window = new ReplayControlSettingsWindow()
            {
                Step = _simulation.Mediator.SimulationData.Step,
                StepSpeed = 1000 / _simulation.Mediator.Interval
            };

            window.Cancel += new EventHandler((_, _) =>
            {
                window.Close();
            });

            window.Save += new EventHandler((_, _) =>
            {
                var mediator = _simulation.Mediator as IReplayMediator;
                if (mediator == null)
                    throw new Exception();
                mediator.JumpToStep(window.Step);
                mediator.SetSpeed(window.StepSpeed);
                window.Close();
            });

            window.ShowDialog();
        }

        #endregion

    }

}
