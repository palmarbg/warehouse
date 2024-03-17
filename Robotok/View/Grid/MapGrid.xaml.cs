using Robotok.MVVM;
using Robotok.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
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
using System.Xml.Linq;

namespace Robotok.View.Grid
{
    /// <summary>
    /// Interaction logic for MapGrid.xaml
    /// </summary>
    public partial class MapGrid : UserControl
    {
        public double Zoom { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public double XOffset { get; set; }
        public double YOffset { get; set; }

        public MapGrid()
        {
            DataContext = this;
            InitializeComponent();

        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if(DataContext is MainWindowViewModel model)
            {
                model.XOffset = (int)e.HorizontalOffset;
                model.YOffset = (int)e.VerticalOffset;
            }
        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;
            RobotLayer.SetDataContext(viewModel);
            GoalLayer.SetDataContext(viewModel);
            BlockLayer.SetDataContext(viewModel);
        }
    }

    #region Converters

    public class SizeToLineObservableCollection : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            // rowcount columncount
            var lines = new SuppressNotifyObservableCollection<ObservableLine>
            {
                SuppressNotify = true
            };

            if (!GridConverterFunctions.ValidateArray(values, 2))
                return lines;

            var rowcount = (int)values[0];
            var columncount = (int)values[1];
            int unit = GridConverterFunctions.unit;

            //for optimisation purposes
            int AmountOfCellsInOneBlock = GridConverterFunctions.AmountOfCellsInOneBlock(rowcount, columncount);

            if (AmountOfCellsInOneBlock > 1)
            {
                rowcount = GridConverterFunctions.NumberOfLines(rowcount, columncount);
                columncount = GridConverterFunctions.NumberOfLines(columncount, rowcount);
                unit *= AmountOfCellsInOneBlock;
            }

            //vertical lines
            for (int i = 1; i <= columncount; i++)
            {
                lines.Add(new ObservableLine
                {
                    X1 = (int)(i * unit),
                    X2 = (int)(i * unit),
                    Y1 = 0,
                    Y2 = (int)(rowcount * unit),
                });
            }
            //horizontal lines
            for (int i = 1; i <= rowcount; i++)
            {
                lines.Add(new ObservableLine
                {
                    Y1 = (int)(i * unit),
                    Y2 = (int)(i * unit),
                    X1 = 0,
                    X2 = (int)(columncount * unit),
                });
            }
            return lines;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SizeToCanvasSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
                return 0;
            var count = (int)value;

            return GridConverterFunctions.MapLength(count);
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    public struct ObservableLine
    {
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }
    }

}
