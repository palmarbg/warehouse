using Robotok.ViewModel;
using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for MapRobot.xaml
    /// </summary>
    public partial class MapRobot : UserControl
    {
        public double Zoom { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public double XOffset { get; set; }
        public double YOffset { get; set; }

        public ObservableCollection<Robot> Robots { get; set; }

        public MapRobot()
        {
            DataContext = this;
            Robots = new ObservableCollection<Robot>();
            InitializeComponent();

        }

        public void SetDataContext(INotifyPropertyChanged viewModel)
        {
            this.DataContext = viewModel;
        }
    }

    #region Converters

    public class DirectionToDotMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null || value == DependencyProperty.UnsetValue)
                return new Thickness(0, 0, 0, 0);
            
            Direction dir = (Direction)value;

            int val = GridConverterFunctions.unit / 2 + 2;

            switch (dir)
            {
                case Direction.Left:
                    return new Thickness(0, 0, val, 0);
                case Direction.Up:
                    return new Thickness(0, 0, 0, val);
                case Direction.Right:
                    return new Thickness(val, 0, 0, 0);
                case Direction.Down:
                default:
                    return new Thickness(0, val, 0, 0);
            }
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
