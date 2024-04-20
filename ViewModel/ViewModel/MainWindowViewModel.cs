using Model.Interfaces;
using Persistence.DataTypes;
using System.Diagnostics;
using ViewModel.MVVM;

namespace ViewModel.ViewModel
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

        public int Interval => Math.Min(_simulation.Interval, 500);

        #endregion

        #region Events

        /// <summary>
        /// Fire with <see cref="Model_RobotsChanged" />
        /// </summary>
        public event EventHandler? RobotsChanged;

        /// <summary>
        /// Fire with <see cref="Model_RobotsMoved" />
        /// </summary>
        public event EventHandler? RobotsMoved;

        /// <summary>
        /// Fire with <see cref="Model_GoalsChanged"/>
        /// </summary>
        public event EventHandler? GoalsChanged;

        /// <summary>
        /// Fire with <see cref="Model_MapLoaded"/>
        /// </summary>
        public event EventHandler? MapLoaded;

        public event EventHandler? LoadSimulation;
        public event EventHandler? SaveSimulation;

        #endregion

        #region Simulation data

        public List<Robot> Robots { get; set; }
        public List<Goal> Goals { get; set; }

        #endregion

        #region DelegateCommands

        /// <summary> Start or pause the simulation </summary>
        public DelegateCommand ToggleSimulationCommand { get; set; }

        /// <summary> Stop the simulation </summary>
        public DelegateCommand StopSimulationCommand { get; set; }

        /// <summary> Pause the simulation </summary>
        public DelegateCommand PauseSimulationCommand { get; set; }

        /// <summary> Go to the start of the simulation </summary>
        public DelegateCommand InitialPositionCommand { get; set; }

        /// <summary> Take one step backward in the simulation </summary>
        public DelegateCommand PreviousStepCommand { get; set; }

        /// <summary> Take one step forward in the simulation </summary>
        public DelegateCommand NextStepCommand { get; set; }

        /// <summary> Display the map after the simulation is over </summary>
        public DelegateCommand FinalPositionCommand { get; set; }


        /// <summary> Load a config file </summary>
        public DelegateCommand LoadSimulationCommand { get; set; }

        /// <summary> Save a log file </summary>
        public DelegateCommand SaveSimulationCommand { get; set; }

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

            simulation.RobotsMoved += new EventHandler((_, _) => Model_RobotsMoved());
            simulation.GoalsChanged += new EventHandler((_, _) => Model_GoalsChanged());
            simulation.SimulationLoaded += new EventHandler((_, _) => Model_SimulationLoaded());

            ToggleSimulationCommand = new DelegateCommand(param => OnToggleSimulation());
            StopSimulationCommand = new DelegateCommand(param => OnSimulationStop());
            PauseSimulationCommand = new DelegateCommand(param => OnSimulationPause());
            InitialPositionCommand = new DelegateCommand(param => OnInitialPosition());
            PreviousStepCommand = new DelegateCommand(param => OnPreviousStep());
            NextStepCommand = new DelegateCommand(param => OnNextStep());
            FinalPositionCommand = new DelegateCommand(param => OnFinalPosition());

            LoadSimulationCommand = new(param => OnLoadSimulation());
            SaveSimulationCommand = new DelegateCommand(param => OnSaveSimulation());

            Model_SimulationLoaded();
        }

        #endregion

        #region Model Event methods

        /// <summary>
        /// Call when the robot collection changed
        /// </summary>
        private void Model_RobotsChanged()
        {
            RobotsChanged?.Invoke(Robots, new EventArgs());
        }

        /// <summary>
        /// Call it when the robots moved, but the collection didn't change
        /// <para />
        /// If the collection changed call <see cref="Model_RobotsChanged"/>
        /// </summary>
        private void Model_RobotsMoved()
        {
            RobotsMoved?.Invoke(Robots, new EventArgs());
        }

        /// <summary>
        /// Call when new goals have been assigned or finished
        /// </summary>
        private void Model_GoalsChanged()
        {
            GoalsChanged?.Invoke(Goals, new EventArgs());
        }

        /// <summary>
        /// Call when blocks on map have been changed
        /// </summary>
        private void Model_MapLoaded()
        {
            MapLoaded?.Invoke(_simulation.SimulationData.Map, new EventArgs());
        }

        /// <summary>
        /// Call when new simulation data have been loaded
        /// </summary>
        private void Model_SimulationLoaded()
        {
            RowCount = _simulation.SimulationData.Map.GetLength(1);
            ColumnCount = _simulation.SimulationData.Map.GetLength(0);

            Robots = _simulation.SimulationData.Robots;
            Goals = _simulation.SimulationData.Goals;

            Model_MapLoaded();
            Model_RobotsChanged();
            Model_GoalsChanged();
        }



        #endregion

        #region ViewModel Event methods

        /// <summary>
        /// View calls when datacontext have been set
        /// </summary>
        public void OnSetDataContext()
        {
            Model_RobotsChanged();
            Model_GoalsChanged();
            Model_MapLoaded();
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
            LoadSimulation?.Invoke(null, new());
        }

        private void OnSaveSimulation()
        {
            SaveSimulation?.Invoke(null, new());
        }

        #endregion


    }
}
