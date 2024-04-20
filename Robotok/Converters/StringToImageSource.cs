using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace View.Converters
{
    public class StringToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage bimage = new();
            bimage.BeginInit();
            bimage.UriSource = new Uri((String)value, UriKind.Relative);
            bimage.EndInit();
            return bimage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            return ((BitmapImage)value).UriSource.ToString();

        }
    }
}
