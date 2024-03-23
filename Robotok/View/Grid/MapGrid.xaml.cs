using Robotok.MVVM;
using Robotok.ViewModel;
using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
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
using System.Xml.Linq;

namespace Robotok.View.Grid
{
    /// <summary>
    /// Interaction logic for MapGrid.xaml
    /// </summary>
    public partial class MapGrid : UserControl
    {
        public MapGrid()
        {
            InitializeComponent();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if(DataContext is MainWindowViewModel model)
            {
                model.XOffset = (int)e.HorizontalOffset;
                model.YOffset = (int)e.VerticalOffset;
            }
        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;
            RobotLayer.SetDataContext(viewModel);
            GoalLayer.SetDataContext(viewModel);
            BlockLayer.SetDataContext(viewModel);
            viewModel.MapLoaded += new EventHandler(DrawLines);
        }

        #region Private methods

        private void DrawLines(object? sender, EventArgs e)
        {
            if (sender == null)
                return;
            ITile[,] map = (ITile[,])sender;
            MapCanvas.Children.Clear();

            int columncount = map.GetLength(0);
            int rowcount = map.GetLength(1);
            int unit = GridConverterFunctions.unit;

            //for optimisation purposes
            int AmountOfCellsInOneBlock = GridConverterFunctions.AmountOfCellsInOneBlock(rowcount, columncount);

            if (AmountOfCellsInOneBlock > 1)
            {
                rowcount = GridConverterFunctions.NumberOfLines(rowcount, columncount);
                columncount = GridConverterFunctions.NumberOfLines(columncount, rowcount);
                unit *= AmountOfCellsInOneBlock;
            }

            SolidColorBrush brush = new(Colors.Black);
            brush.Freeze();

            //vertical lines
            for (int i = 1; i <= columncount; i++)
            {
                System.Windows.Shapes.Line line = new()
                {
                    X1 = (int)(i * unit),
                    X2 = (int)(i * unit),
                    Y1 = 0,
                    Y2 = (int)(rowcount * unit),
                    Stroke = brush,
                    StrokeThickness = 1
                };
                MapCanvas.Children.Add(line);
            }
            //horizontal lines
            for (int i = 1; i <= rowcount; i++)
            {
                System.Windows.Shapes.Line line = new()
                {
                    Y1 = (int)(i * unit),
                    Y2 = (int)(i * unit),
                    X1 = 0,
                    X2 = (int)(columncount * unit),
                    Stroke = brush,
                    StrokeThickness = 1
                };
                MapCanvas.Children.Add(line);
            }

        }

        #endregion
    }

}
