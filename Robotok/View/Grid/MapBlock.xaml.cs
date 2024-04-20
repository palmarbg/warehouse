using Persistence.DataTypes;
using Robotok.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Block = Persistence.DataTypes.Block;


namespace Robotok.View.Grid
{
    /// <summary>
    /// Interaction logic for MapBlock.xaml
    /// </summary>
    public partial class MapBlock : Canvas
    {

        public MapBlock()
        {
            InitializeComponent();
        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;
            viewModel.MapLoaded += new EventHandler(
                (s, e) => App.Current?.Dispatcher.Invoke((Action)delegate { RefreshMap(s, e); })
                );
        }

        #region Private methods
        private void RefreshMap(object? sender, EventArgs e)
        {
            if (sender == null)
                return;
            ITile[,] map = (ITile[,])sender;

            MapCanvas.Children.Clear();

            SolidColorBrush brush = new(Colors.Black);
            brush.Freeze();

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] is not Block)
                        continue;

                    int start = x;
                    int end = x;
                    while (x + 1 < map.GetLength(0) && (map[x + 1, y] is Block)) // if the next tile is also a block
                    {
                        end = ++x;
                    }
                    int width = end - start + 1;
                    System.Windows.Controls.Grid grid = new()
                    {
                        Width = GridConverterFunctions.unit * width,
                        Height = GridConverterFunctions.unit,
                        Margin = new Thickness(
                        GridConverterFunctions.unit * start,
                        GridConverterFunctions.unit * y,
                        0,
                        0)
                    };

                    System.Windows.Shapes.Rectangle rectangle = new()
                    {
                        Fill = brush
                    };

                    grid.Children.Add(rectangle);
                    MapCanvas.Children.Add(grid);
                }
            }
        }
        #endregion
    }
}
