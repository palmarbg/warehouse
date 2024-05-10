using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
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
    /// Interaction logic for SimulationControlSettingsWindow.xaml
    /// </summary>
    public partial class SimulationControlSettingsWindow : Window, INotifyPropertyChanged
    {
        private Int32 _step = 0;
        private Int32 _stepInterval = 0;

        public event EventHandler? Save;
        public event EventHandler? Cancel;
        public event PropertyChangedEventHandler? PropertyChanged;

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
        public Int32 StepInterval
        {
            get { return _stepInterval; }
            set
            {
                if (_stepInterval != value)
                {
                    _stepInterval = value;
                    OnPropertyChanged();
                }
            }
        }

        public SimulationControlSettingsWindow()
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
}
