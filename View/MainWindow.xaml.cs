using Model.DataTypes;
using System.Diagnostics;
using System.Windows;
using ViewModel.ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;
            _mapGridContainer.SetDataContext(viewModel);
            _zoomSlider.SetDataContext(viewModel);
            _menubar.SetDataContext(viewModel);
            viewModel.OnSetDataContext();

            viewModel.RobotsChanged += new EventHandler((_, _) => RefreshSimulationStepCounter(0));
            viewModel.RobotsMoved += new EventHandler<RobotsMovedEventArgs>((_, arg) => RefreshSimulationStepCounter(arg.SimulationStep));
        }

        private void RefreshSimulationStepCounter(int step)
        {
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                _stepCounterLabel.Text = $"Lépés: {step}";
            });
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(DataContext is  MainWindowViewModel viewModel)
            {
                switch (e.Key)
                {
                    case System.Windows.Input.Key.Space:
                        viewModel.ToggleSimulationCommand.Execute(this);
                        break;
                    case System.Windows.Input.Key.O:
                        viewModel.OpenSettingsCommand.Execute(this);
                        break;
                }
            }
            
            e.Handled = true;
        }
    }
}
