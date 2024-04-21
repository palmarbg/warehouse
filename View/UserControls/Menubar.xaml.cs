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

            SetCommandBinding(_playButton, nameof(viewModel.ToggleSimulationCommand), viewModel);
            SetCommandBinding(_stopButton, nameof(viewModel.StopSimulationCommand), viewModel);
            SetCommandBinding(_startButton, nameof(viewModel.InitialPositionCommand), viewModel);
            SetCommandBinding(_backButton, nameof(viewModel.PreviousStepCommand), viewModel);
            SetCommandBinding(_nextButton, nameof(viewModel.NextStepCommand), viewModel);
            SetCommandBinding(_endButton, nameof(viewModel.FinalPositionCommand), viewModel);
            SetCommandBinding(_settingButton, nameof(viewModel.OpenReplaySettingsCommand), viewModel);

            SetCommandBinding(_loadSimulationMenuItem, nameof(viewModel.LoadSimulationCommand), viewModel);
            SetCommandBinding(_saveSimulationMenuItem, nameof(viewModel.SaveSimulationCommand), viewModel);

            _playButton.Click += new RoutedEventHandler((_, _) =>
            {
                _playButton.IconSrc = _playButton.IconSrc == "Icons/pause.png" ? "Icons/play.png" : "Icons/pause.png";
                _playButton.LabelText = _playButton.IconSrc == "Icons/pause.png" ? "Pause" : "Start";
            });

            _stopButton.Click += new RoutedEventHandler((_, _) =>
            {
                _playButton.IconSrc = "Icons/play.png";
                _playButton.LabelText = "Start";
            });

            _startButton.Click += new RoutedEventHandler((_, _) =>
            {
                _playButton.IconSrc = "Icons/play.png";
                _playButton.LabelText = "Start";
            });

            _loadSimulationMenuItem.Click += new RoutedEventHandler((_, _) =>
            {
                _playButton.IconSrc = "Icons/play.png";
                _playButton.LabelText = "Start";
            });
        }

        public void SetCommandBinding(Button btn, string path, MainWindowViewModel viewModel)
        {
            Binding binding = new()
            {
                Source = viewModel,
                Path = new PropertyPath(path),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(btn, Button.CommandProperty, binding);
        }

        public void SetCommandBinding(MenuItem btn, string path, MainWindowViewModel viewModel)
        {
            Binding binding = new()
            {
                Source = viewModel,
                Path = new PropertyPath(path),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(btn, MenuItem.CommandProperty, binding);
        }
    }
}
