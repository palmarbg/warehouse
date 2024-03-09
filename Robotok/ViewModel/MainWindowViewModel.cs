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
        private double _zoom;
        private int _row;
        private int _column;
        private int _xoffset;
        private int _yoffset;

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

        public ObservableCollection<Robot> Robots { get; set; }
        public ObservableCollection<Goal> Goals { get; set; }
        public ObservableCollection<ObservableBlock> Blocks { get; set; }

        public bool[,] Map;

        #endregion

        public MainWindowViewModel()
        {
            _zoom=1.0;
            _row=500;
            _column=500;
            _xoffset = 0;
            _yoffset = 0;
            
            Random random = new Random();

            Robots = new ObservableCollection<Robot>();
            Goals = new ObservableCollection<Goal>();
            Blocks = new ObservableCollection<ObservableBlock>();

            Map = new bool[ColumnCount, RowCount];

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    Map[i, j] = random.Next(100) < 95;
                }
            }

            var values = Enum.GetValues(typeof(Direction));
            for(int i = 0; i < 500; i++)
            {
                Robots.Add(new Robot {  Rotation = (Direction)values.GetValue(random.Next(values.Length)),
                                        Position = new Position {
                                            X = random.Next(0, ColumnCount),
                                            Y = random.Next(0, RowCount)
                                        } });
                Goals.Add(new Goal {    Position = new Position {
                                            X = random.Next(0, ColumnCount),
                                            Y = random.Next(0, RowCount)
                                        } });
            }

            CalculateBlocks();
        }

        public void CalculateBlocks()
        {
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


    }
}
