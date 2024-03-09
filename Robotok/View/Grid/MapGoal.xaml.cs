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
    /// Interaction logic for MapGoal.xaml
    /// </summary>
    public partial class MapGoal : UserControl
    {
        public double Zoom { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }

        public ObservableCollection<Goal> Goals { get; set; }

        public MapGoal()
        {
            DataContext = this;
            Goals = new ObservableCollection<Goal>();
            InitializeComponent();

        }

        public void SetDataContext(INotifyPropertyChanged viewModel)
        {
            this.DataContext = viewModel;
        }
    }
}
