using Robotok.ViewModel;
using System.Windows;

namespace Robotok.View
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
        }
    }
}
