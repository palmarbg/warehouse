using Microsoft.VisualBasic;
using Robotok.MVVM;
using Robotok.View.Grid;
using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Robotok.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        
        private double _zoom;
        private int _row;
        private int _column;
        private int _xoffset;
        private int _yoffset;

        public bool[,] Map;

        #endregion

        #region Properties

        public double Zoom
        {
            get { return _zoom; }
            set
            {
                if (_zoom != value)
                {
                    _zoom = value;
                    OnPropertyChanged();
                }
            }
        }


        public int RowCount
        {
            get { return _row; }
            set
            {
                if (_row != value)
                {
                    _row = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ColumnCount
        {
            get { return _column; }
            set
            {
                if (_column != value)
                {
                    _column = value;
                    OnPropertyChanged();
                }
            }
        }

        public int XOffset
        {
            get { return _xoffset; }
            set
            {
                if (_xoffset != value)
                {
                    _xoffset = value;
                    OnPropertyChanged();
                }
            }
        }

        public int YOffset
        {
            get { return _yoffset; }
            set
            {
                if (_yoffset != value)
                {
                    _yoffset = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollectionWrapper<Goal> ObservableGoals { get; set; }
        public ObservableCollectionWrapper<ObservableBlock> ObservableBlocks { get; set; }
        

        #endregion

        #region Events

        /// <summary>
        /// Fire with <see cref="OnRobotsChanged" />
        /// </summary>
        public event EventHandler RobotsChanged;

        /// <summary>
        /// Fire with <see cref="OnRobotsMoved" />
        /// </summary>
        public event EventHandler RobotsMoved;

        #endregion

        #region Simulation data

        //These will be deleted when the viewmodel get them from the simulation

        public List<Robot> Robots { get; set; }
        public List<Goal> Goals { get; set; }
        public List<ObservableBlock> Blocks { get; set; }

        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            _zoom=1.0;
            _row=200;
            _column=200;
            _xoffset = 0;
            _yoffset = 0;

            Robots = new List<Robot>();
            Goals = new List<Goal>();
            Blocks = new List<ObservableBlock>();

            Map = new bool[200, 200];

            ObservableBlocks = new(Blocks);
            ObservableGoals = new(Goals);

            PopulateMap();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Populate map with blocks, goals and robots for demo
        /// </summary>
        private async void PopulateMap()
        {
            await Task.Run(() =>
            {
                Random random = new Random();

                for (int i = 0; i < Map.GetLength(0); i++)
                {
                    for (int j = 0; j < Map.GetLength(1); j++)
                    {
                        Map[i, j] = random.Next(100) < 95;
                    }
                }

                var values = Enum.GetValues(typeof(Direction));
                for (int i = 0; i < 2000; i++)
                {
                    Robots.Add(new Robot
                    {
                        Rotation = (Direction)values.GetValue(random.Next(values.Length))!,
                        Position = new Position
                        {
                            X = random.Next(0, ColumnCount),
                            Y = random.Next(0, RowCount)
                        }
                    });
                    Goals.Add(new Goal
                    {
                        Position = new Position
                        {
                            X = random.Next(0, ColumnCount),
                            Y = random.Next(0, RowCount)
                        }
                    });
                }

                CalculateBlocks();

                ObservableBlocks.OnCollectionChanged();
                ObservableGoals.OnCollectionChanged();

                OnRobotsChanged();


                Thread.Sleep(3000);
                foreach (var robot in Robots)
                {
                    robot.Position = new Position { X = robot.Position.X - 1, Y = robot.Position.Y };
                }

                OnRobotsMoved();

                Thread.Sleep(500);
                foreach (var robot in Robots)
                {
                    robot.Rotation = Direction.Down;
                }

                OnRobotsMoved();

                Thread.Sleep(500);
                foreach (var robot in Robots)
                {
                    robot.Position = new Position { X = robot.Position.X - 1, Y = robot.Position.Y };
                }

                OnRobotsMoved();
            });
        }

        /// <summary>
        /// Groups continuous blocks together in rows
        /// </summary>
        public void CalculateBlocks()
        {
            Blocks.Clear();
            int j = 0;
            int start=0, end=0;
            for(int i = 0; i < Map.GetLength(0); i++)
            {
                for(j=0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j]) // if empty tile
                        continue;
                    start= j;
                    end = j;
                    while(j+1< Map.GetLength(1) && !Map[i, j+1]) // if the next tile is also a block
                    {
                        end = ++j;
                    }
                    Blocks.Add((new ObservableBlock { X = start, Y = i, Width=end-start+1}));
                }
            }
        }

        #endregion

        #region Event methods

        /// <summary>
        /// Call when the robot collection changed
        /// </summary>
        public void OnRobotsChanged()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                RobotsChanged.Invoke(Robots, new EventArgs());
            });
        }

        /// <summary>
        /// Call when robots moved, but the collection didn't changed
        /// <para />
        /// If the collection changed call <see cref="OnRobotsChanged"/> before
        /// </summary>
        public void OnRobotsMoved()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                RobotsMoved.Invoke(Robots, new EventArgs());
            });
        }

        #endregion


    }
}
