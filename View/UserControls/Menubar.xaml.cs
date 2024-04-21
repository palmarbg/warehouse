using Model.DataTypes;
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

            viewModel.SimulationStateChanged += new EventHandler<SimulationState>((_,s) => OnSimulationStateChanged(s));

            SetCommandBinding(_playButton, nameof(viewModel.ToggleSimulationCommand), viewModel);
            SetCommandBinding(_stopButton, nameof(viewModel.StopSimulationCommand), viewModel);
            SetCommandBinding(_startButton, nameof(viewModel.InitialPositionCommand), viewModel);
            SetCommandBinding(_backButton, nameof(viewModel.PreviousStepCommand), viewModel);
            SetCommandBinding(_nextButton, nameof(viewModel.NextStepCommand), viewModel);
            SetCommandBinding(_endButton, nameof(viewModel.FinalPositionCommand), viewModel);
            SetCommandBinding(_settingButton, nameof(viewModel.OpenReplaySettingsCommand), viewModel);

            SetCommandBinding(_loadSimulationMenuItem, nameof(viewModel.LoadSimulationCommand), viewModel);
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

        private void OnSimulationStateChanged(SimulationState simulationState)
        {
            Debug.WriteLine($"IsRunning: {simulationState.IsSimulationRunning}");
            Debug.WriteLine($"IsEnded: {simulationState.IsSimulationEnded}");

            _playButton.IconSrc = !simulationState.IsSimulationRunning ? "Icons/play.png" : "Icons/pause.png";
            _playButton.LabelText = !simulationState.IsSimulationRunning ? "Start" : "Pause";

            _playButton.IsEnabled = true;
            _stopButton.IsEnabled = simulationState.IsSimulationStarted;
            _startButton.IsEnabled = simulationState.IsSimulationStarted;
            _backButton.IsEnabled = simulationState.IsSimulationStarted && !simulationState.IsSimulationRunning;
            _nextButton.IsEnabled = !simulationState.IsSimulationRunning;
            _endButton.IsEnabled = !simulationState.IsSimulationRunning;
            _settingButton.IsEnabled = !simulationState.IsSimulationRunning;
        }
    }
}
