using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ViewModel.MVVM;
namespace View.Grid
{
    #region Numberstrip converters 
    public class SizeToStringObservableCollection : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //zoom count offset observablecollection
            var labelTexts = (SuppressNotifyObservableCollection<string>)values[3];

            if (!GridConverterFunctions.ValidateArray(values, 4))
                return labelTexts;

            double zoom = (double)values[0];
            int count = (int)values[1];
            int offset = (int)values[2];

            for (int i = 0; i < labelTexts.Count; i++)
                labelTexts[i] = string.Empty;

            labelTexts.SuppressNotify = true;
            while (GridConverterFunctions.NumberOfLabelsOnScreen_Max(zoom) > labelTexts.Count)
                labelTexts.Add(string.Empty);
            labelTexts.SuppressNotify = false;


            var groupedAmountInOneBlock = GridConverterFunctions.AmountOfNumbersInGroupedLabel(zoom);

            int start = GridConverterFunctions.NumberOfLabelsToOmit(offset, zoom);
            int end = Math.Min(start + GridConverterFunctions.NumberOfLabelsOnScreen_Max(zoom), count);


            if (groupedAmountInOneBlock == 1)
            {
                for (int i = start + 1; i <= end; i++)
                    labelTexts[i - 1 - start] = i.ToString();
                return labelTexts;
            }
            int numberOfBlocks = GridConverterFunctions.NumberOfGroupedLabels(count, zoom);
            end = Math.Min(start + GridConverterFunctions.NumberOfLabelsOnScreen_Max(zoom), numberOfBlocks);

            for (int i = start; i < end; i++)
                labelTexts[i - start] = ($"{groupedAmountInOneBlock * i + 1}:");//-{groupedAmountInOneBlock * (i + 1)}
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

            return GridConverterFunctions.GroupedLabelLength(zoom);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Map layer converters

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
}
