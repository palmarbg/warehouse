using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ViewModel.MVVM;
using ViewModel.ViewModel;

namespace View.Grid
{
    /// <summary>
    /// Interaction logic for NumberStrip.xaml
    /// </summary>
    public partial class VerticalNumberStrip : UserControl
    {
        public SuppressNotifyObservableCollection<string> LabelTexts { get; set; }

        public VerticalNumberStrip()
        {
            LabelTexts = [];
            InitializeComponent();
        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;
        }

        private void _ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            //just to disable scrolling
            e.Handled = true;
        }

        private void _ScrollViewer_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //just to disable scrolling
            e.Handled = true;
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
