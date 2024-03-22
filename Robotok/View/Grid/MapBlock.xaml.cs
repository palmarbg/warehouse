using Robotok.MVVM;
using Robotok.ViewModel;
using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
using Block = RobotokModel.Model.Block;

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
            viewModel.MapLoaded += new EventHandler(RefreshMap);
        }

        #region Private methods
        private void RefreshMap(object? sender, EventArgs e)
        {
            if (sender == null)
                return;
            ITile[,] map = (ITile[,])sender;
            MapCanvas.Children.Clear();
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
                        Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                        Margin = new Thickness(0.5)
                    };

                    grid.Children.Add(rectangle);
                    MapCanvas.Children.Add(grid);
                }
            }
        }
        #endregion
    }

    public class ObservableBlock
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
    }
}
