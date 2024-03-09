using Robotok.MVVM;
using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

        #endregion

        public MainWindowViewModel()
        {
            _zoom=1.0;
            _row=200;
            _column=200;
            _xoffset = 0;
            _yoffset = 0;
            Robots = new ObservableCollection<Robot>();
            Random random = new Random();
            var values = Enum.GetValues(typeof(Direction));
            for(int i = 0; i < 500; i++)
            {
                Robots.Add(new Robot { Rotation = (Direction)values.GetValue(random.Next(values.Length)), Position = new Position { X = random.Next(0, ColumnCount), Y = random.Next(0, RowCount) } });
            }
            /*Robots.Add(new Robot { Rotation = Direction.Left, Position = new Position{ X=1, Y=5 } });
            Robots.Add(new Robot { Rotation = Direction.Up, Position = new Position{ X=2, Y=5 } });
            Robots.Add(new Robot { Rotation = Direction.Right, Position = new Position{ X=3, Y=5 } });
            Robots.Add(new Robot { Rotation = Direction.Down, Position = new Position { X = 4, Y = 5 } });*/
        }
    }
}
