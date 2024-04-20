using ViewModel.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace View.Grid
{
    /// <summary>
    /// Interaction logic for MapGridContainer.xaml
    /// </summary>
    public partial class MapGridContainer : UserControl, INotifyPropertyChanged
    {

        #region Properties


        #endregion

        public MapGridContainer()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] String? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;
            MapGrid.SetDataContext(viewModel);
            VerticalNumberStrip.SetDataContext(viewModel);
            HorizontalNumberStrip.SetDataContext(viewModel);
        }
    }


}
