using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace Robotok.View.Grid
{
    /// <summary>
    /// Interaction logic for NumberStrip.xaml
    /// </summary>
    public partial class HorizontalNumberStrip : UserControl
    {
        public int ColumnCount { get; set; }
        public double Zoom { get; set; }
        public int XOffset { get; set; }
        public int Thickness { get; set; }
        public HorizontalNumberStrip()
        {
            InitializeComponent();
        }

        public void SetDataContext(INotifyPropertyChanged viewModel)
        {
            this.DataContext = viewModel;
        }
    }

    #region Converters
    public class SizeToStringObservableCollection : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //zoom count offset
            var labelTexts = new ObservableCollection<string>();

            if(!GridConverterFunctions.ValidateArray(values, 3))
                return labelTexts;

            double zoom = (double)values[0];
            int count = (int)values[1];
            int offset = (int)values[2];


            var groupedAmountInOneBlock = GridConverterFunctions.AmountOfNumbersInOneLabel(zoom);

            Debug.WriteLine(GridConverterFunctions.NumberOfLabelsToOmit(offset, zoom));

            int start = GridConverterFunctions.NumberOfLabelsToOmit(offset, zoom);
            int end = Math.Min(start+100, count);


            if (groupedAmountInOneBlock == 1)
            {
                for (int i = start+1; i <= end; i++)
                    labelTexts.Add(i.ToString());
                return labelTexts;
            }
            int numberOfBlocks = GridConverterFunctions.NumberOfLabels(count, zoom);
            end = Math.Min(start + 100, numberOfBlocks);

            for (int i = start; i < end; i++)
                labelTexts.Add($"{groupedAmountInOneBlock * i + 1}:");//-{groupedAmountInOneBlock * (i + 1)}
            return labelTexts;

        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StripSizeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
                return 0;

            double zoom = (double)value;
            
            return GridConverterFunctions.LabelLength(zoom);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HorizontalMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // offset zoom
            if (!GridConverterFunctions.ValidateArray(values,2))
                return new Thickness(0,0,0,0);
            int offset = (int)values[0];
            double zoom = (double)values[1];

            offset = GridConverterFunctions.OmittedOffset(offset, zoom);

            return new Thickness(-(int)offset, 0, (int)offset, 0);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

}
