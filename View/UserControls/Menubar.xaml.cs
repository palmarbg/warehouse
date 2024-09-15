﻿using Model.DataTypes;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ViewModel.ViewModel;

namespace View.UserControls
{
    /// <summary>
    /// Interaction logic for Menubar.xaml
    /// </summary>
    public partial class Menubar : UserControl
    {
        public Menubar()
        {
            InitializeComponent();
        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;

            viewModel.SimulationStateChanged += new EventHandler<SimulationStateEventArgs>((_,arg) =>
                this.Dispatcher.Invoke(() =>
                    OnSimulationStateChanged(arg)
                ));

            SetCommandBinding(_playButton, nameof(viewModel.ToggleSimulationCommand), viewModel);
            SetCommandBinding(_stopButton, nameof(viewModel.StopSimulationCommand), viewModel);
            SetCommandBinding(_startButton, nameof(viewModel.InitialPositionCommand), viewModel);
            SetCommandBinding(_backButton, nameof(viewModel.PreviousStepCommand), viewModel);
            SetCommandBinding(_nextButton, nameof(viewModel.NextStepCommand), viewModel);
            SetCommandBinding(_endButton, nameof(viewModel.FinalPositionCommand), viewModel);
            SetCommandBinding(_settingButton, nameof(viewModel.OpenSettingsCommand), viewModel);

            SetCommandBinding(_loadSimulationMenuItem, nameof(viewModel.LoadSimulationCommand), viewModel);
            SetCommandBinding(_loadReplayMenuItem, nameof(viewModel.LoadReplayCommand), viewModel);


            SetCommandBinding(_startNewSimulationMenuItem, nameof(viewModel.StartNewSimulationCommand), viewModel);
            SetCommandBinding(_saveSimulationMenuItem, nameof(viewModel.SaveSimulationCommand), viewModel);

        }

        private void SetCommandBinding(Button btn, string path, MainWindowViewModel viewModel)
        {
            Binding binding = new()
            {
                Source = viewModel,
                Path = new PropertyPath(path),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(btn, Button.CommandProperty, binding);
        }

        private void SetCommandBinding(MenuItem btn, string path, MainWindowViewModel viewModel)
        {
            Binding binding = new()
            {
                Source = viewModel,
                Path = new PropertyPath(path),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(btn, MenuItem.CommandProperty, binding);
        }

        private void OnSimulationStateChanged(SimulationStateEventArgs arg)
        {
            var simulationState = arg.SimulationState;
            
            _playButton.IconSrc     = simulationState.IsSimulationRunning ? "Icons/pause.png" : "Icons/play.png" ;
            _playButton.LabelText   = simulationState.IsSimulationRunning ? "Pause" : "Start";

            //canexecute should be in viewmodel and binded to buttons

            if(!arg.IsReplayMode)
            {
                _fileMenuItem.IsEnabled = !simulationState.IsSimulationRunning;
                _simulationMenuItem.IsEnabled = !simulationState.IsSimulationRunning;
                _saveSimulationMenuItem.IsEnabled = true;

                _playButton.IsEnabled = true;
                _stopButton.IsEnabled = simulationState.State != SimulationStates.SimulationEnded;
                _startButton.IsEnabled = !simulationState.IsSimulationRunning;
                _backButton.IsEnabled = false;
                _nextButton.IsEnabled = false;
                _endButton.IsEnabled = false;
                _settingButton.IsEnabled = !simulationState.IsSimulationRunning;
                return;
            }
            _fileMenuItem.IsEnabled = !simulationState.IsSimulationRunning;
            _simulationMenuItem.IsEnabled = !simulationState.IsSimulationRunning;

            _saveSimulationMenuItem.IsEnabled = false;

            _playButton.IsEnabled = true;
            _stopButton.IsEnabled = simulationState.State != SimulationStates.SimulationEnded;
            _startButton.IsEnabled = true;
            _backButton.IsEnabled = !simulationState.IsSimulationRunning;
            _nextButton.IsEnabled = !simulationState.IsSimulationRunning;
            _endButton.IsEnabled = !simulationState.IsSimulationRunning;
            _settingButton.IsEnabled = !simulationState.IsSimulationRunning;
        }
    }
}
