using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace View.Windows
{
    /// <summary>
    /// Interaction logic for ReplayControlSettings.xaml
    /// </summary>
    public partial class ReplayControlSettingsWindow : Window, INotifyPropertyChanged
    {
        private Int32 _step = 0;
        private float _stepSpeed = 0;
        public Int32 Step
        {
            get { return _step; }
            set
            {
                if (_step != value)
                {
                    _step = value;
                    OnPropertyChanged();
                }
            }
        }
        public float StepSpeed
        {
            get { return _stepSpeed; }
            set
            {
                if (_stepSpeed != value)
                {
                    _stepSpeed = value;
                    OnPropertyChanged();
                }
            }
        }

        public event EventHandler? Save;
        public event EventHandler? Cancel;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ReplayControlSettingsWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = Regex.Replace(((TextBox)sender).Text, @"[^\d]", "");
            ((TextBox)sender).Text = s;
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            Save?.Invoke(this, EventArgs.Empty);
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            Cancel?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] String? propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    #region Converters
    public class SimulationSpeedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
                return 0.0;
            return (double)Math.Pow((float)value, 0.25);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
                return 0.0;
            return (float)Math.Pow((double)value, 4);
        }
    }

    #endregion

}
