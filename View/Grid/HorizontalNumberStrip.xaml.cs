using ViewModel.MVVM;
using ViewModel.ViewModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace View.Grid
{
    /// <summary>
    /// Interaction logic for NumberStrip.xaml
    /// </summary>
    public partial class HorizontalNumberStrip : UserControl
    {
        public SuppressNotifyObservableCollection<string> LabelTexts { get; set; }
        public HorizontalNumberStrip()
        {
            LabelTexts = [];
            InitializeComponent();
        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;
        }
    }

    #region Converters

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
