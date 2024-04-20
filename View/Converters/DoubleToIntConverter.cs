using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace View.Converters
{
    internal class DoubleToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null || value == DependencyProperty.UnsetValue)
                return 0;
            return (int)((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
