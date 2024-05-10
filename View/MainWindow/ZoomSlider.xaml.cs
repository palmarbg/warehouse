using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ViewModel.ViewModel;

namespace View.Grid
{
    /// <summary>
    /// Interaction logic for ZoomSlider.xaml
    /// </summary>
    public partial class ZoomSlider : UserControl
    {
        public double Zoom { get; set; }
        public ZoomSlider()
        {
            InitializeComponent();
        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;
        }
    }

    #region Converters
    public class ZoomScaleToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
                return 0.0;
            return Math.Round(Math.Pow((double)value, 0.25), 2, MidpointRounding.AwayFromZero);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
                return 0.0;
            return Math.Round(Math.Pow((double)value, 4), 2, MidpointRounding.AwayFromZero);
        }
    }
    #endregion
}
