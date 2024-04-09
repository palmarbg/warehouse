using Microsoft.VisualBasic;
using Microsoft.Win32;
using Robotok.MVVM;
using Robotok.View.Grid;
using RobotokModel.Model;
using RobotokModel.Model.Interfaces;
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

        private ISimulation _simulation;

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

        #region Events

        /// <summary>
        /// Fire with <see cref="OnRobotsChanged" />
        /// </summary>
        public event EventHandler? RobotsChanged;

        /// <summary>
        /// Fire with <see cref="OnRobotsMoved" />
        /// </summary>
        public event EventHandler? RobotsMoved;

        /// <summary>
        /// Fire with <see cref="OnGoalsChanged"/>
        /// </summary>
        public event EventHandler? GoalsChanged;

        /// <summary>
        /// Fire with <see cref="OnMapLoaded"/>
        /// </summary>
        public event EventHandler? MapLoaded;

        #endregion

        #region Simulation data

        public List<Robot> Robots { get; set; }
        public List<Goal> Goals { get; set; }

        #endregion

        #region DelegateCommands

        /// <summary> Start or pause the simulation </summary>
        public DelegateCommand ToggleSimulation { get; set; }

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


        /// <summary> Load a config file </summary>
        public DelegateCommand LoadSimulation { get; set; }

        #endregion

        #region Constructor
        public MainWindowViewModel(ISimulation simulation)
        {
            _simulation = simulation;
            _zoom = 1.0;
            _row = 0;
            _column = 0;
            _xoffset = 0;
            _yoffset = 0;

            Robots = [];
            Goals = [];

            simulation.RobotsMoved += new EventHandler((_,_) => OnRobotsMoved());
            simulation.GoalsChanged += new EventHandler((_,_) => OnGoalsChanged());
            simulation.SimulationLoaded += new EventHandler((_, _) => OnSimulationLoaded());            

            ToggleSimulation = new DelegateCommand(param => OnToggleSimulation());
            StopSimulation = new DelegateCommand(param => OnSimulationStop());
            PauseSimulation = new DelegateCommand(param => OnSimulationPause());
            InitialPosition = new DelegateCommand(param => OnInitialPosition());
            PreviousStep = new DelegateCommand(param => OnPreviousStep());
            NextStep = new DelegateCommand(param => OnNextStep());
            FinalPosition = new DelegateCommand(param => OnFinalPosition());

            LoadSimulation = new(param => OnLoadSimulation());

            OnSimulationLoaded();
        }

        #endregion

        #region Model Event methods

        /// <summary>
        /// Call when the robot collection changed
        /// </summary>
        private void OnRobotsChanged()
        {
            App.Current?.Dispatcher.Invoke((Action)delegate
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
            App.Current?.Dispatcher.Invoke((Action)delegate
            {
                RobotsMoved?.Invoke(Robots, new EventArgs());
            });
        }

        /// <summary>
        /// Call when new goals have been assigned or finished
        /// </summary>
        private void OnGoalsChanged()
        {
            App.Current?.Dispatcher.Invoke((Action)delegate
            {
                GoalsChanged?.Invoke(Goals, new EventArgs());
            });
        }

        /// <summary>
        /// Call when blocks on map have been changed
        /// </summary>
        private void OnMapLoaded()
        {
            App.Current?.Dispatcher.Invoke((Action)delegate
            {
                MapLoaded?.Invoke(_simulation.SimulationData.Map, new EventArgs());
            });
        }

        /// <summary>
        /// Call when new simulation data have been loaded
        /// </summary>
        private void OnSimulationLoaded()
        {
            RowCount = _simulation.SimulationData.Map.GetLength(1);
            ColumnCount = _simulation.SimulationData.Map.GetLength(0);

            Robots = _simulation.SimulationData.Robots;
            Goals = _simulation.SimulationData.Goals;

            OnMapLoaded();
            OnRobotsChanged();
            OnGoalsChanged();
        }

        #endregion

        #region ViewModel Event methods

        /// <summary>
        /// View calls when datacontext have been set
        /// </summary>
        public void OnSetDataContext()
        {
            OnRobotsChanged();
            OnGoalsChanged();
            OnMapLoaded();
        }

        private void OnToggleSimulation()
        {
            Debug.WriteLine("simulation start");
            if (_simulation.State.IsSimulationRunning)
                _simulation.Mediator.PauseSimulation();
            else
                _simulation.Mediator.StartSimulation();
        }

        private void OnSimulationStop()
        {
            Debug.WriteLine("simulation stop");
            _simulation.Mediator.StopSimulation();
        }

        private void OnSimulationPause()
        {
            Debug.WriteLine("simulation pause");
        }
        private void OnInitialPosition()
        {
            Debug.WriteLine("first step");
            _simulation.Mediator.SetInitialState();
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


        private void OnLoadSimulation()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Konfigurációs fájl betöltése";
            openFileDialog.Filter = "Config file|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                _simulation.Mediator.LoadSimulation(openFileDialog.FileName);
            }
        }

        #endregion


    }
}
