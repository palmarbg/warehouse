using Model.DataTypes;
using Persistence.DataTypes;
using System;
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
    public partial class RobotLayer : Canvas
    {
        private static int ROUND_INTERVAL = 50;

        private int _roundsToRefreshBuffer = 0;
        private int[] _robotDistanceFromView = null!;

        private bool _isRefreshing = false;
        private bool _toRefresh = false;

        private bool _isRefreshingBuffer = false;
        private bool _toRefreshBuffer = false;


        private Position _center = new Position { X = 0, Y = 0 };
        private int rx = 0;
        private int ry = 0;

        private DateTime? _lastAnimationEnd = null!;

        private List<Robot> _robots = null!;

        public RobotLayer()
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

            viewModel.RobotsMoved += new EventHandler<RobotsMovedEventArgs>(
                (s, arg) => App.Current?.Dispatcher.Invoke((Action)delegate {
                        Debug.WriteLine("here we goo");
                    if(s == null || _robots == null)
                        return;
                    if (arg.IsJumped)
                    {
                        Debug.WriteLine("here we goo!!!");
                        FlushBuffer();
                    }
                    RefreshRobots(arg.TimeSpan);
                })
                );

            viewModel.WindowChanged += new EventHandler<MainWindowViewModel>((_, vm) => OnWindowChanged(vm));

            OnWindowChanged(viewModel);
        }

        #region Private Methods
        private void AddRobots(object? sender, EventArgs e)
        {
            if (sender == null)
                return;

            _robots = (List<Robot>)sender;

            _isRefreshing = false;

            MapCanvas.Children.Clear();

            foreach (Robot robot in _robots)
            {
                var grid = GetWPFRobot(robot);
                MapCanvas.Children.Add(grid);
            }

            FlushBuffer(_robots.Count);
            RefreshRobots(TimeSpan.Zero);
        }

        private void RefreshRobots(TimeSpan timeSpan)
        {
            if (_isRefreshing)
            {
                _toRefresh = true;
                return;
            }

            _isRefreshing = true;

            _lastAnimationEnd = DateTime.Now + timeSpan;

            if(_roundsToRefreshBuffer == 0)
                RefreshBuffer();
            else
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
                    Robot robot = _robots[i];

                    int x = robot.Position.X;
                    int y = robot.Position.Y;

                    ThicknessAnimation marginAnimation = new(
                        new Thickness(GridConverterFunctions.unit * x, GridConverterFunctions.unit * y, 0, 0),
                        new Duration(timeSpan))
                    {
                        From = grid.Margin
                    };
                    grid.BeginAnimation(MarginProperty, marginAnimation);

                    //rotation
                    (grid.Children[2] as FrameworkElement)!.Margin = _margins[robot.Rotation];

                }
            }

            _isRefreshing = false;

            if (_toRefresh)
            {
                _toRefresh = false;
                timeSpan = (TimeSpan)(_lastAnimationEnd - DateTime.Now);
                if (_lastAnimationEnd < DateTime.Now)
                    timeSpan = TimeSpan.Zero;
                RefreshRobots(timeSpan);
            }
        }

        #endregion

        #region Event methods

        private void OnWindowChanged(MainWindowViewModel vm)
        {
            int groupedLabel = (int)GridConverterFunctions.AmountOfNumbersInLabelGroup(vm.Zoom);

            rx = GridConverterFunctions.NumberOfLabelGroupsOnScreen_Horizontal(vm.Zoom) / 2 * groupedLabel;
            ry = GridConverterFunctions.NumberOfLabelGroupsOnScreen_Vertical(vm.Zoom) / 2 * groupedLabel;

            //get the top left position
            int x = GridConverterFunctions.NumberOfLabelsToOmit(vm.XOffset, vm.Zoom) * groupedLabel;
            int y = GridConverterFunctions.NumberOfLabelsToOmit(vm.YOffset, vm.Zoom) * groupedLabel;

            _center = new Position
            {
                X = x + rx,
                Y = y + ry
            };


            if (_lastAnimationEnd == null)
                return;

            RefreshBuffer();

            TimeSpan timeSpan = (TimeSpan)(_lastAnimationEnd - DateTime.Now);
            if (_lastAnimationEnd < DateTime.Now)
                timeSpan = TimeSpan.Zero;

            RefreshRobots(timeSpan);
        }

        #endregion

        #region WPF Resources

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

        #region Buffer Refresh

        private void RefreshBuffer()
        {
            if (_isRefreshingBuffer)
            {
                _toRefreshBuffer = true;
                return;
            }

            _isRefreshingBuffer = true;

            for (int i = 0; i < _robots.Count; i++)
            {
                
                //prevent robots jumping across the screen
                if (_robotDistanceFromView[i] > 0 && _robotDistanceFromView[i] <= ROUND_INTERVAL)
                {
                    var element = MapCanvas.Children[i];
                    if (element is System.Windows.Controls.Grid grid)
                    {
                        Robot robot = _robots[i];

                        int x = robot.Position.X;
                        int y = robot.Position.Y;

                        ThicknessAnimation marginAnimation = new(
                        new Thickness(GridConverterFunctions.unit * x, GridConverterFunctions.unit * y, 0, 0),
                        new Duration(TimeSpan.Zero))
                        {
                            From = grid.Margin
                        };
                        grid.BeginAnimation(MarginProperty, marginAnimation);

                    }
                }
                

                //doesn't have to calculate distance again if far enough
                if (_robotDistanceFromView[i] < 0)
                    _robotDistanceFromView[i] = _robotDistanceFromView[i] + ROUND_INTERVAL;
                else
                    _robotDistanceFromView[i] = _robotDistanceFromView[i] - ROUND_INTERVAL;


                if (Math.Abs(_robotDistanceFromView[i]) <= ROUND_INTERVAL)
                    _robotDistanceFromView[i] =
                        Math.Max(0,
                            ManhattanDistance(_robots[i].Position)
                                - ROUND_INTERVAL);
            }

            _isRefreshingBuffer = false;

            if (_toRefreshBuffer)
            {
                _toRefreshBuffer = false;
                RefreshBuffer();
            }

            _roundsToRefreshBuffer = ROUND_INTERVAL;
        }

        private void FlushBuffer(int? robotCount = null)
        {
            _toRefreshBuffer = false;
            _isRefreshing = false;
            _isRefreshingBuffer = false;
            _roundsToRefreshBuffer = 0;

            if(robotCount == null)
            {
                _robotDistanceFromView = new int[_robotDistanceFromView.Length];
            } else
            {
                _robotDistanceFromView = new int[(int)robotCount];
            }
            

            //prevent fantom robots
            for (int i = 0; i < _robots.Count; i++)
            {
                var element = MapCanvas.Children[i];
                if (element is System.Windows.Controls.Grid grid)
                {
                    Robot robot = _robots[i];

                    int x = robot.Position.X;
                    int y = robot.Position.Y;

                    ThicknessAnimation marginAnimation = new(
                        new Thickness(GridConverterFunctions.unit * x, GridConverterFunctions.unit * y, 0, 0),
                        new Duration(TimeSpan.Zero))
                    {
                        From = grid.Margin
                    };
                    grid.BeginAnimation(MarginProperty, marginAnimation);

                }
            }

            RefreshBuffer();
        }

        private int ManhattanDistance(Position point)
        {
            int dx = Math.Abs(_center.X - point.X) - rx;
            int dy = Math.Abs(_center.Y - point.Y) - ry;

            if (dx < 0 && dy < 0)
                return Math.Min(dx, dy);

            return Math.Max(0, dx) + Math.Max(dy, 0);
        }

        #endregion

        #region WPF Robot

        private static System.Windows.Controls.Grid GetWPFRobot(Robot robot)
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

            return grid;
        }

        #endregion
    }
}
