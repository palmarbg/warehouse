using Robotok.MVVM;
using Robotok.ViewModel;
using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Robotok.View.Grid
{
    /// <summary>
    /// Interaction logic for MapBlock.xaml
    /// </summary>
    public partial class MapBlock : UserControl
    {
        public double Zoom { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }

        public ObservableCollectionWrapper<ObservableBlock> ObservableBlocks { get; set; }

        public MapBlock()
        {
            DataContext = this;
            ObservableBlocks = new ([]);
            InitializeComponent();

        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;
        }
    }

    public class ObservableBlock
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
    }
}
