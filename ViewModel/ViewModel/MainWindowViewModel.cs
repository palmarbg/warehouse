using Model.DataTypes;
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
                    OnWindowChanged();
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
                    OnWindowChanged();
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
                    OnWindowChanged();
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Fire with <see cref="Model_RobotsChanged" />
        /// </summary>
        public event EventHandler? RobotsChanged;

        /// <summary>
        /// Fire with <see cref="Model_RobotsMoved" />
        /// </summary>
        public event EventHandler<RobotsMovedEventArgs>? RobotsMoved;

        /// <summary>
        /// Fire with <see cref="Model_GoalChanged"/>
        /// </summary>
        public event EventHandler<Goal?>? GoalChanged;

        /// <summary>
        /// Fire with <see cref="Model_MapLoaded"/>
        /// </summary>
        public event EventHandler? MapLoaded;
        
        /// <summary>
        /// Fire with <see cref="Model_SimulationStateChanged"/>
        /// </summary>
        public event EventHandler<SimulationStateEventArgs>? SimulationStateChanged;

        public event EventHandler<SimulationStepEventArgs>? SimulationStep;

        public event EventHandler? OpenSettings;
        public event EventHandler? LoadSimulation;
        public event EventHandler? LoadReplay;
        public event EventHandler? SaveSimulation;

        public event EventHandler<MainWindowViewModel>? WindowChanged;

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

        /// <summary> Open window for setting simulation speed and position </summary>
        public DelegateCommand OpenSettingsCommand { get; set; }

        /// <summary> Load a config file </summary>
        public DelegateCommand LoadSimulationCommand { get; set; }
        
        /// <summary> Load a log file </summary>
        public DelegateCommand LoadReplayCommand { get; set; }

        /// <summary> Init new simulation </summary>
        public DelegateCommand StartNewSimulationCommand { get; set; }

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

            simulation.RobotsMoved += new EventHandler<RobotsMovedEventArgs>((_, t) => Model_RobotsMoved(t));
            simulation.GoalChanged += new EventHandler<Goal?>((r, goal) => Model_GoalChanged((Robot)r!, goal));
            simulation.SimulationLoaded += new EventHandler((_, _) => Model_SimulationLoaded());
            simulation.SimulationStateChanged += new EventHandler<SimulationStateEventArgs>((_, arg) => Model_SimulationStateChanged(arg));
            simulation.SimulationStep += new EventHandler<SimulationStepEventArgs>((_, arg) => Model_SimulationStep(arg));

            ToggleSimulationCommand = new DelegateCommand(param => OnToggleSimulation());
            StopSimulationCommand = new DelegateCommand(param => OnSimulationStop());
            PauseSimulationCommand = new DelegateCommand(param => OnSimulationPause());
            InitialPositionCommand = new DelegateCommand(param => OnInitialPosition());
            PreviousStepCommand = new DelegateCommand(param => OnPreviousStep());
            NextStepCommand = new DelegateCommand(param => OnNextStep());
            FinalPositionCommand = new DelegateCommand(param => OnFinalPosition());

            OpenSettingsCommand = new DelegateCommand(param => OnOpenReplaySettings());
            LoadSimulationCommand = new(param => OnLoadSimulation());
            LoadReplayCommand = new(param => OnLoadReplay());
            StartNewSimulationCommand = new DelegateCommand(param => OnStartNewSimulation());
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
            RobotsChanged?.Invoke(Robots, new System.EventArgs());
        }

        /// <summary>
        /// Call it when the robots moved, but the collection didn't change
        /// <para />
        /// If the collection changed call <see cref="Model_RobotsChanged"/>
        /// </summary>
        private void Model_RobotsMoved(RobotsMovedEventArgs args)
        {
            RobotsMoved?.Invoke(Robots, args);
        }

        /// <summary>
        /// Call when new goals have been assigned or finished
        /// </summary>
        private void Model_GoalChanged(Robot robot, Goal? goal)
        {
            GoalChanged?.Invoke(robot, goal);
        }

        /// <summary>
        /// Call when blocks on map have been changed
        /// </summary>
        private void Model_MapLoaded()
        {
            MapLoaded?.Invoke(_simulation.SimulationData.Map, new System.EventArgs());
        }

        private void Model_SimulationStateChanged(SimulationStateEventArgs arg)
        {
            SimulationStateChanged?.Invoke(null, arg);
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
        }

        private void Model_SimulationStep(SimulationStepEventArgs arg)
        {
            SimulationStep?.Invoke(null, arg);
        }

        #endregion

        #region ViewModel Event methods

        private void OnToggleSimulation()
        {
            if (_simulation.SimulationState.IsSimulationRunning)
                _simulation.PauseSimulation();
            else
                _simulation.StartSimulation();
        }

        private void OnSimulationStop()
        {
            _simulation.StopSimulation();
        }

        private void OnSimulationPause()
        {
            _simulation.PauseSimulation();
        }
        private void OnInitialPosition()
        {
            _simulation.SetInitialPosition();
        }

        private void OnPreviousStep()
        {
            _simulation.StepBackward();
        }

        private void OnNextStep()
        {
            _simulation.StepForward();
        }

        private void OnFinalPosition()
        {
            _simulation.JumpToEnd();
        }

        private void OnOpenReplaySettings()
        {
            OpenSettings?.Invoke(null, new());
        }

        private void OnStartNewSimulation()
        {
            _simulation.StartNewSimulation();
        }

        private void OnLoadSimulation()
        {
            LoadSimulation?.Invoke(null, new());
            _simulation.OnSimulationStateChanged(_simulation.SimulationState);
        }

        private void OnLoadReplay()
        {
            LoadReplay?.Invoke(null, new());
            _simulation.OnSimulationStateChanged(_simulation.SimulationState);
        }

        private void OnSaveSimulation()
        {
            SaveSimulation?.Invoke(null, new());
        }

        #endregion

        #region View Event methods

        /// <summary>
        /// View calls it when datacontext have been set
        /// </summary>
        public void OnSetDataContext()
        {
            Model_RobotsChanged();
            Model_MapLoaded();
            _simulation.OnSimulationStateChanged(_simulation.SimulationState);
        }

        public void OnWindowChanged()
        {
            WindowChanged?.Invoke(this, this);
        }

        #endregion
    }
}
