using Robotok.View.Grid;
using Robotok.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Robotok.View.UserControls
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

            SetCommandBinding(_playButton,  "ToggleSimulation",  viewModel);
            SetCommandBinding(_stopButton,  "StopSimulation",   viewModel);
            SetCommandBinding(_startButton, "InitialPosition",  viewModel);
            SetCommandBinding(_backButton,  "PreviousStep",     viewModel);
            SetCommandBinding(_nextButton,  "NextStep",         viewModel);
            SetCommandBinding(_endButton,   "FinalPosition",    viewModel);

            SetCommandBinding(_loadSimulationMenuItem, "LoadSimulation", viewModel);

            _playButton.Click += new RoutedEventHandler((_,_) => {
                _playButton.IconSrc = _playButton.IconSrc == "Icons/pause.png" ? "Icons/play.png" : "Icons/pause.png";
                _playButton.LabelText = _playButton.IconSrc == "Icons/pause.png" ? "Pause" : "Start";
            });

            _stopButton.Click += new RoutedEventHandler((_, _) => {
                _playButton.IconSrc = "Icons/play.png";
                _playButton.LabelText = "Start";
            });

            _startButton.Click += new RoutedEventHandler((_, _) => {
                _playButton.IconSrc = "Icons/play.png";
                _playButton.LabelText = "Start";
            });

            _loadSimulationMenuItem.Click += new RoutedEventHandler((_, _) => {
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
