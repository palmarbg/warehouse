using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Robotok.View.Converters
{
    public class DoubleToStringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue ||
                parameter == null || parameter == DependencyProperty.UnsetValue)
                return 0;

            NumberFormatInfo nfi = new();
            nfi.NumberDecimalSeparator = ".";

            return Math.Round((double)value, System.Convert.ToInt32((String)parameter), MidpointRounding.AwayFromZero).ToString(nfi);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
