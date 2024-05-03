using Persistence.DataTypes;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
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
        private int _roundsToRefreshBuffer = 0;
        private int[] _robotDistanceFromView = null!;

        private static int ROUND_INTERVAL = 50;

        private Position _center = new Position { X = 0, Y = 0 };
        private int rx = 0;
        private int ry = 0;

        private DateTime? _lastArgDate = null!;
        private List<Robot> _lastArgRobot = null!;

        public MapRobot()
        {
            blackBrush.Freeze();
            blueBrush.Freeze();
            InitializeComponent();
        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;

            viewModel.RobotsChanged += new EventHandler(
                (s, e) => App.Current?.Dispatcher.Invoke((Action)delegate { AddRobots(s, e); })
                );

            viewModel.RobotsMoved += new EventHandler<TimeSpan>(
                (s, t) => App.Current?.Dispatcher.Invoke((Action)delegate { RefreshRobots(s, t); })
                );

            viewModel.WindowChanged += new EventHandler<MainWindowViewModel>((_, vm) => OnWindowChanged(vm));

            OnWindowChanged(viewModel);
        }

        #region Private Methods
        private void AddRobots(object? sender, EventArgs e)
        {
            if (sender == null)
                return;

            List<Robot> robots = (List<Robot>)sender;

            _roundsToRefreshBuffer = 0;
            _robotDistanceFromView = new int[robots.Count];

            MapCanvas.Children.Clear();

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
                    Margin = ellipseThickness
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
                    Margin = _margins[robot.Rotation]
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

            _lastArgRobot = robots;
            _lastArgDate = DateTime.Now + timeSpan;

            if(_roundsToRefreshBuffer == 0)
            {
                for (int i = 0; i < robots.Count; i++)
                {
                    if (_robotDistanceFromView[i] != 0 && _robotDistanceFromView[i] <= ROUND_INTERVAL)
                    {
                        var element = MapCanvas.Children[i];
                        if (element is System.Windows.Controls.Grid grid)
                        {
                            Robot robot = robots[i];

                            int x = robot.Position.X;
                            int y = robot.Position.Y;

                            grid.Margin = new Thickness(GridConverterFunctions.unit * x, GridConverterFunctions.unit * y, 0, 0);

                        }
                    }
                    
                    if (_robotDistanceFromView[i] < 0)
                        _robotDistanceFromView[i] = _robotDistanceFromView[i] + ROUND_INTERVAL;
                    else
                        _robotDistanceFromView[i] = _robotDistanceFromView[i] - ROUND_INTERVAL;


                    if (Math.Abs(_robotDistanceFromView[i]) <= ROUND_INTERVAL)
                        _robotDistanceFromView[i] =
                            Math.Max( 0,
                                ManhattanDistance(robots[i].Position)
                                    - ROUND_INTERVAL);
                }
                _roundsToRefreshBuffer = ROUND_INTERVAL;
            }

            _roundsToRefreshBuffer--;

            if (timeSpan > TimeSpan.FromMilliseconds(500))
                timeSpan = TimeSpan.FromMilliseconds(500);

            for (int i = 0; i < MapCanvas.Children.Count; i++)
            {
                if (_robotDistanceFromView[i] > 0)
                    continue;

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

                    ((FrameworkElement)grid.Children[2]).Margin = _margins[robot.Rotation];

                }
            }
        }

        private void OnWindowChanged(MainWindowViewModel vm)
        {
            //Debug.WriteLine("Window is");
            //Debug.WriteLine($"Offset {vm.XOffset}:{vm.YOffset}");
            //Debug.WriteLine($"Zoom {vm.Zoom}");

            int groupedLabel = (int)GridConverterFunctions.AmountOfNumbersInGroupedLabel(vm.Zoom);

            rx = GridConverterFunctions.NumberOfLabelsOnScreen_Horizontal(vm.Zoom) / 2 * groupedLabel;
            ry = GridConverterFunctions.NumberOfLabelsOnScreen_Vertical(vm.Zoom) / 2 * groupedLabel;

            //get the top left position
            int x = GridConverterFunctions.NumberOfLabelsToOmit(vm.XOffset, vm.Zoom) * groupedLabel;
            int y = GridConverterFunctions.NumberOfLabelsToOmit(vm.YOffset, vm.Zoom) * groupedLabel;

            _center = new Position
            {
                X = x + rx,
                Y = y + ry
            };

            _roundsToRefreshBuffer = 0;

            if (_lastArgDate == null || _lastArgRobot == null)
                return;

            TimeSpan timeSpan = (TimeSpan)(_lastArgDate - DateTime.Now);
            if(_lastArgDate <  DateTime.Now)
                timeSpan = TimeSpan.Zero;
            RefreshRobots(_lastArgRobot, timeSpan);
            //Debug.WriteLine("final is");
            //Debug.WriteLine($"CENTER IS {_center.X} - {_center.Y}");
            //Debug.WriteLine($"TOPLEFT IS {x}:{y}");
            //Debug.WriteLine($"RADIUS IS {rx}:{ry}");
        }

        #endregion

        #region Private Utils

        public int ManhattanDistance(Position point)
        {
            int dx = Math.Abs(_center.X - point.X) - rx;
            int dy = Math.Abs(_center.Y - point.Y) - ry;

            if(dx < 0 && dy < 0)
                return Math.Min(dx, dy);

            return Math.Max(0,dx) + Math.Max(dy, 0);
        }

        private static Dictionary<Direction, Thickness> _margins = new Dictionary<Direction, Thickness>
        {
            { Direction.Right,  new Thickness(GridConverterFunctions.unit * 0.7 + 2, 0, 0, 0) },
            { Direction.Down,   new Thickness(0, GridConverterFunctions.unit * 0.7 + 2, 0, 0) },
            { Direction.Left,   new Thickness(0, 0, GridConverterFunctions.unit * 0.7 + 2, 0) },
            { Direction.Up,     new Thickness(0, 0, 0, GridConverterFunctions.unit * 0.7 + 2) },
        };

        private static SolidColorBrush blackBrush = new(Colors.Black);
        private static SolidColorBrush blueBrush = new(Color.FromRgb(9, 194, 248));
        private static Thickness ellipseThickness = new Thickness(2);

        #endregion
    }
}
