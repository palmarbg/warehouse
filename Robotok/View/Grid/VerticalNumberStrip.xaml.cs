using Robotok.MVVM;
using Robotok.View.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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

namespace Robotok.View.Grid
{
    /// <summary>
    /// Interaction logic for NumberStrip.xaml
    /// </summary>
    public partial class VerticalNumberStrip : UserControl
    {
        public int RowCount { get; set; }
        public double Zoom { get; set; }
        public int YOffset { get; set; }
        public int Thickness { get; set; }

        public SuppressNotifyObservableCollection<string> LabelTexts { get; set; }

        public VerticalNumberStrip()
        {
            LabelTexts = new SuppressNotifyObservableCollection<string>();
            InitializeComponent();
        }

        public void SetDataContext(INotifyPropertyChanged viewModel)
        {
            this.DataContext = viewModel;
        }
    }

    #region Converters

    public class VerticalMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // offset zoom
            if (!GridConverterFunctions.ValidateArray(values, 2))
                return new Thickness(0, 0, 0, 0);
            int offset = (int)values[0];
            double zoom = (double)values[1];

            offset = GridConverterFunctions.OmittedOffset(offset, zoom);

            return new Thickness(0, -(int)offset, 0, (int)offset);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

}
