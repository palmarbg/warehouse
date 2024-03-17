using Robotok.MVVM;
using Robotok.ViewModel;
using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Robotok.View.Grid
{
    /// <summary>
    /// Interaction logic for MapRobot.xaml
    /// </summary>
    public partial class MapRobot : UserControl
    {
        public double Zoom { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }

        public ObservableCollectionWrapper<Robot> ObservableRobots { get; set; }

        public MapRobot()
        {
            DataContext = this;
            ObservableRobots = new (new List<Robot>());
            InitializeComponent();

        }

        public void SetDataContext(INotifyPropertyChanged viewModel)
        {
            this.DataContext = viewModel;
            ((MainWindowViewModel)viewModel).RobotsChanged += new EventHandler(AddRobots);
            ((MainWindowViewModel)viewModel).RobotsMoved += new EventHandler(RefreshRobots);
        }

        public void AddRobots(object? sender, EventArgs e)
        {
            if (sender == null)
                return;
            List<Robot> robots = (List<Robot>)sender;

            MapCanvas.Children.Clear();
            foreach (Robot robot in robots)
            {
                System.Windows.Controls.Grid grid = new();
                grid.Width = GridConverterFunctions.unit;
                grid.Height = GridConverterFunctions.unit;
                grid.ToolTip = robot.Id;
                ToolTipService.SetInitialShowDelay(grid, 0);
                ToolTipService.SetShowDuration(grid, 9999999);
                ToolTipService.SetBetweenShowDelay(grid, 0);

                grid.Margin = new Thickness(
                    GridConverterFunctions.unit * robot.Position.X,
                    GridConverterFunctions.unit * robot.Position.Y,
                    0,
                    0);

                System.Windows.Shapes.Ellipse ellipse = new();
                ellipse.Fill = new SolidColorBrush(Color.FromRgb(9, 194, 248));
                ellipse.Margin = new Thickness(2);

                System.Windows.Controls.TextBlock textBlock = new();
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.FontSize = 12;
                textBlock.Text = robot.Id.ToString();

                System.Windows.Shapes.Ellipse dot = new();
                dot.Fill = new SolidColorBrush(Colors.Black);
                dot.Width = 6;
                dot.Height = 6;
                dot.Margin = DirectionToDotMargin(robot.Rotation);


                
                grid.Children.Add(ellipse);
                grid.Children.Add(textBlock);
                grid.Children.Add(dot);
                MapCanvas.Children.Add(grid);
            }
        }

        public void RefreshRobots(object? sender, EventArgs e)
        {
            if (sender == null)
                return;
            List<Robot> robots = (List<Robot>)sender;

            for(int i=0; i< MapCanvas.Children.Count;i++)
            {
                var element = MapCanvas.Children[i];
                if(element is System.Windows.Controls.Grid)
                {
                    var grid = (System.Windows.Controls.Grid)element;

                    Robot robot = robots[i];

                    int x = robot.Position.X;
                    int y = robot.Position.Y;

                    ThicknessAnimation marginAnimation = new ThicknessAnimation(
                        new Thickness(GridConverterFunctions.unit * x, GridConverterFunctions.unit * y, 0, 0),
                        new Duration(TimeSpan.FromSeconds(.5)));
                    marginAnimation.From = grid.Margin;
                    grid.BeginAnimation(System.Windows.Controls.Grid.MarginProperty, marginAnimation);

                    ((FrameworkElement)grid.Children[2]).Margin = DirectionToDotMargin(robot.Rotation);
                    
                    Debug.WriteLine(grid.ToolTip.ToString());
                }
            }
        }

        private Thickness DirectionToDotMargin(Direction dir)
        {
            int val = (int)(GridConverterFunctions.unit * 0.7) + 2;

            switch (dir)
            {
                case Direction.Left:
                    return new Thickness(0, 0, val, 0);
                case Direction.Up:
                    return new Thickness(0, 0, 0, val);
                case Direction.Right:
                    return new Thickness(val, 0, 0, 0);
                case Direction.Down:
                default:
                    return new Thickness(0, val, 0, 0);
            }
        }

    }
}
