using Persistence.DataTypes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ViewModel.ViewModel;

namespace View.Grid
{
    /// <summary>
    /// Interaction logic for MapRobot.xaml
    /// </summary>
    public partial class MapRobot : Canvas
    {
        private MainWindowViewModel _viewModel = null!;
        public MapRobot()
        {
            InitializeComponent();
        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            this.DataContext = viewModel;
            viewModel.RobotsChanged += new EventHandler(
                (s, e) => App.Current?.Dispatcher.Invoke((Action)delegate { AddRobots(s, e); })
                );
            viewModel.RobotsMoved += new EventHandler<TimeSpan>(
                (s, t) => App.Current?.Dispatcher.Invoke((Action)delegate { RefreshRobots(s, t); })
                );
        }

        #region Private Methods
        private void AddRobots(object? sender, EventArgs e)
        {
            if (sender == null)
                return;

            List<Robot> robots = (List<Robot>)sender;

            MapCanvas.Children.Clear();

            SolidColorBrush blackBrush = new(Colors.Black);
            blackBrush.Freeze();

            SolidColorBrush blueBrush = new(Color.FromRgb(9, 194, 248));
            blueBrush.Freeze();

            foreach (Robot robot in robots)
            {
                System.Windows.Controls.Grid grid = new()
                {
                    Width = GridConverterFunctions.unit,
                    Height = GridConverterFunctions.unit,
                    ToolTip = robot.Id,
                    Margin = new Thickness(
                        GridConverterFunctions.unit * robot.Position.X,
                        GridConverterFunctions.unit * robot.Position.Y,
                        0,
                        0)
                };
                ToolTipService.SetInitialShowDelay(grid, 0);
                ToolTipService.SetShowDuration(grid, 9999999);
                ToolTipService.SetBetweenShowDelay(grid, 0);

                System.Windows.Shapes.Ellipse ellipse = new()
                {
                    Fill = blueBrush,
                    Margin = new Thickness(2)
                };

                System.Windows.Controls.TextBlock textBlock = new()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 12,
                    Text = robot.Id.ToString()
                };

                System.Windows.Shapes.Ellipse dot = new()
                {
                    Fill = blackBrush,
                    Width = 6,
                    Height = 6,
                    Margin = DirectionToDotMargin(robot.Rotation)
                };



                grid.Children.Add(ellipse);
                grid.Children.Add(textBlock);
                grid.Children.Add(dot);
                MapCanvas.Children.Add(grid);
            }
        }

        private void RefreshRobots(object? sender, TimeSpan timeSpan)
        {
            if (sender == null)
                return;
            List<Robot> robots = (List<Robot>)sender;

            if (timeSpan > TimeSpan.FromMilliseconds(500))
                timeSpan = TimeSpan.FromMilliseconds(500);

            for (int i = 0; i < MapCanvas.Children.Count; i++)
            {
                var element = MapCanvas.Children[i];
                if (element is System.Windows.Controls.Grid grid)
                {
                    Robot robot = robots[i];

                    int x = robot.Position.X;
                    int y = robot.Position.Y;

                    ThicknessAnimation marginAnimation = new(
                        new Thickness(GridConverterFunctions.unit * x, GridConverterFunctions.unit * y, 0, 0),
                        new Duration(timeSpan))
                    {
                        From = grid.Margin
                    };
                    grid.BeginAnimation(System.Windows.Controls.Grid.MarginProperty, marginAnimation);

                    ((FrameworkElement)grid.Children[2]).Margin = DirectionToDotMargin(robot.Rotation);

                }
            }
        }

        private Thickness DirectionToDotMargin(Direction dir)
        {
            int val = (int)(GridConverterFunctions.unit * 0.7) + 2;

            return dir switch
            {
                Direction.Left => new Thickness(0, 0, val, 0),
                Direction.Up => new Thickness(0, 0, 0, val),
                Direction.Right => new Thickness(val, 0, 0, 0),
                _ => new Thickness(0, val, 0, 0),
            };
        }
        #endregion

    }
}
