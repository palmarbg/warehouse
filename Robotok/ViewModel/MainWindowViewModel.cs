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

        private Simulation _simulation;

        private double _zoom;
        private int _row;
        private int _column;
        private int _xoffset;
        private int _yoffset;

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

        #endregion

        #region Observable collections
        
        public ObservableCollectionWrapper<Goal> ObservableGoals { get; set; }
        public ObservableCollectionWrapper<ObservableBlock> ObservableBlocks { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Fire with <see cref="OnRobotsChanged" />
        /// </summary>
        public event EventHandler? RobotsChanged;

        /// <summary>
        /// Fire with <see cref="OnRobotsMoved" />
        /// </summary>
        public event EventHandler? RobotsMoved;

        #endregion

        #region Simulation data

        //These will be deleted when the viewmodel get them from the simulation

        public List<Robot> Robots { get; set; }
        public List<Goal> Goals { get; set; }
        public List<ObservableBlock> Blocks { get; set; }

        #endregion

        #region DelegateCommands

        /// <summary> Start the simulation </summary>
        public DelegateCommand StartSimulation { get; set; }

        /// <summary> Stop the simulation </summary>
        public DelegateCommand StopSimulation { get; set; }

        /// <summary> Pause the simulation </summary>
        public DelegateCommand PauseSimulation { get; set; }

        /// <summary> Go to the start of the simulation </summary>
        public DelegateCommand InitialPosition { get; set; }

        /// <summary> Take one step backward in the simulation </summary>
        public DelegateCommand PreviousStep { get; set; }

        /// <summary> Take one step forward in the simulation </summary>
        public DelegateCommand NextStep { get; set; }

        /// <summary> Display the map after the simulation is over </summary>
        public DelegateCommand FinalPosition { get; set; }


        #endregion

        #region Constructor
        public MainWindowViewModel(Simulation simulation)
        {
            _simulation = simulation;

            simulation.RobotsChanged += new EventHandler((_,_) => OnRobotsChanged());
            simulation.RobotsMoved += new EventHandler((_,_) => OnRobotsMoved());
            simulation.GoalsChanged += new EventHandler((_,_) => ObservableGoals?.OnCollectionChanged());

            _zoom=1.0;
            _row=simulation.simulationData.Map.GetLength(1);
            _column=simulation.simulationData.Map.GetLength(0);
            _xoffset = 0;
            _yoffset = 0;

            Robots = simulation.simulationData.Robots;

            Goals = [];
            Blocks = [];

            StartSimulation = new DelegateCommand(param => OnSimulationStart());
            StopSimulation = new DelegateCommand(param => OnSimulationStop());
            PauseSimulation = new DelegateCommand(param => OnSimulationPause());
            InitialPosition = new DelegateCommand(param => OnInitialPosition());
            PreviousStep = new DelegateCommand(param => OnPreviousStep());
            NextStep = new DelegateCommand(param => OnNextStep());
            FinalPosition = new DelegateCommand(param => OnFinalPosition());

            ObservableBlocks = new(Blocks);
            ObservableGoals = new(Goals);

            CalculateBlocks(simulation.simulationData.Map);
            OnRobotsChanged();
        }

        #endregion

        #region Public methods

        public void OnSetDataContext()
        {
            OnRobotsChanged();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Groups continuous blocks together in rows
        /// </summary>
        private void CalculateBlocks(ITile[,] Map)
        {
            Blocks.Clear();
            for (int y = 0; y < Map.GetLength(1); y++)
            {
                Debug.WriteLine(Map[y, 0] is Block);
                for (int x = 0; x < Map.GetLength(0); x++)
                {
                    if (Map[x, y] is not Block)
                        continue;

                    int start = x;
                    int end = x;
                    while (x + 1 < Map.GetLength(0) && (Map[x + 1, y] is Block)) // if the next tile is also a block
                    {
                        end = ++x;
                    }
                    Blocks.Add((new ObservableBlock { X = start, Y = y, Width = end - start + 1 }));
                }
            }
            ObservableBlocks.OnCollectionChanged();
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
                RobotsChanged?.Invoke(Robots, new EventArgs());
            });
        }

        /// <summary>
        /// Call it when the robots moved, but the collection didn't change
        /// <para />
        /// If the collection changed call <see cref="OnRobotsChanged"/>
        /// </summary>
        private void OnRobotsMoved()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                RobotsMoved?.Invoke(Robots, new EventArgs());
            });
        }

        private void OnSimulationStart()
        {
            Debug.WriteLine("simulation start");
            _simulation.StartSimulation();
        }

        private void OnSimulationStop()
        {
            Debug.WriteLine("simulation stop");
            _simulation.StopSimulation();
        }

        private void OnSimulationPause()
        {
            Debug.WriteLine("simulation pause");
        }
        private void OnInitialPosition()
        {
            Debug.WriteLine("first step");
        }

        private void OnPreviousStep()
        {
            Debug.WriteLine("prev step");
        }

        private void OnNextStep()
        {
            Debug.WriteLine("next step");
        }

        private void OnFinalPosition()
        {
            Debug.WriteLine("last step");
        }

        #endregion


    }
}
