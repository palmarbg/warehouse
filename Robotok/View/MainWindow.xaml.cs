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
using System.Windows.Shapes;

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
            //_mapGridContainer.DataContext = this.DataContext;

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
