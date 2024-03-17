using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Robotok.View.UserControls
{
    /// <summary>
    /// Interaction logic for MenuIcon.xaml
    /// </summary>
    public partial class MenuIcon : Button, INotifyPropertyChanged
    {
        private string _label;
        private string _iconSrc;

        #region Properties
        public string LabelText
        {
            get { return _label; }
            set
            {
                if (_label != value)
                {
                    _label = value;
                    OnPropertyChanged();
                }

            }
        }
        public string IconSrc
        {
            get { return _iconSrc; }
            set
            {
                if (_iconSrc != value)
                {
                    _iconSrc = value;
                    OnPropertyChanged();
                }

            }
        }

        #endregion

        public MenuIcon()
        {
            _label = string.Empty;
            _iconSrc = string.Empty;
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] String? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
