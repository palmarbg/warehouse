using Model.DataTypes;
using Model.Interfaces;
using Persistence.DataTypes;
using Persistence.Interfaces;
using System.Diagnostics;

namespace Model.Mediators
{
    public abstract class AbstractMediator : IMediator
    {

        #region Protected Fields

        protected readonly IServiceLocator _serviceLocator;
        protected readonly ISimulation _simulation;

        protected SimulationState _simulationState;
        protected SimulationData _simulationData = null!;


        protected readonly System.Timers.Timer Timer;
        protected int _interval;
        protected DateTime _timeBeforeController;

        protected IDataAccess _dataAccess = null!;
        protected IController _controller = null!;
        protected IExecutor _executor = null!;

        protected int _lastStep;

        #endregion

        #region Properties

        public SimulationData SimulationData => _simulationData;
        public SimulationState SimulationState => _simulationState;
        public virtual int Interval => _interval;
        public string MapFileName {  get; protected set; }

        #endregion

        #region Constructor

        public AbstractMediator(ISimulation simulation, IServiceLocator serviceLocator, string mapFileName)
        {
            _serviceLocator = serviceLocator;

            this._simulation = simulation;

            _interval = 500;
            Timer = new System.Timers.Timer
            {
                Interval = _interval,
                Enabled = false
            };

            _simulationState = new SimulationState();
            _simulationState.SimulationStateChanged +=
                new EventHandler<SimulationState>(
                    (_, _) => simulation.OnSimulationStateChanged(_simulationState)
                );
            MapFileName = mapFileName;

            _lastStep = 10000;

        }

        #endregion

        #region Public methods

        public void StartSimulation()
        {
            if (_simulationState.IsSimulationRunning)
                return;

            InitSimulationIfNeeded();

            ContinueSimulation();
        }

        public void StopSimulation()
        {
            //TODO: which states can be escaped?

            Timer.Stop();
            _simulationState.State = SimulationStates.SimulationEnded;

            (_controller as IDisposableController)?.Dispose();

            //TODO: escape running controller/executor
            _simulation.OnSimulationFinished();
            //SetInitialState();
        }

        public void PauseSimulation()
        {
            Timer.Stop();
            if(_simulationState.State == SimulationStates.Waiting)
                _simulationState.State = SimulationStates.SimulationPaused;
        }

        public void SetInitialPosition()
        {
            Timer.Stop();

            _simulationState.State = SimulationStates.SimulationPaused;
            
            _simulationData = _dataAccess.GetInitialSimulationData();
            _simulationData.ControllerName = _controller.Name;

            (_controller as IDisposableController)?.Dispose();

            _controller = _controller.NewInstance();
            _executor = _executor.NewInstance(_simulationData);

            _controller.FinishedTask += new EventHandler<IControllerEventArgs>((sender, e) =>
            {
                if (_controller != sender)
                    return;
                OnTaskFinished(e);
            });

            _controller.InitializationFinished += new EventHandler((sender, e) =>
            {
                if (_controller != sender)
                    return;
                if (Timer.Enabled)
                    _simulationState.State = SimulationStates.Waiting;
                else
                    _simulationState.State = SimulationStates.SimulationPaused;
            });

            _simulation.OnSimulationLoaded();

        }

        #endregion

        #region Private methods

        /// <summary>
        /// Called when the Controller finishes the calculations
        /// </summary>
        /// <param name="e">Contains the calculated operations</param>
        private void OnTaskFinished(IControllerEventArgs e)
        {
            Debug.WriteLine("TASK FINISHED");

            _simulationState.State = SimulationStates.ExecutingMoves;

            var elapsedTime = (DateTime.Now - _timeBeforeController).TotalSeconds;
            _executor.ExecuteOperations(e.robotOperations, (float)elapsedTime);
            _simulationData.Step++;

            if (Timer.Enabled)
                _simulationState.State = SimulationStates.Waiting;
            else
                _simulationState.State = SimulationStates.SimulationPaused;

            _simulation.OnRobotsMoved(new RobotsMovedEventArgs()
            {
                SimulationStep = _simulationData.Step,
                IsJumped = false,
                RobotOperations = e.robotOperations,
                TimeSpan = TimeSpan.FromMilliseconds(_interval)
            });
        }

        private void ContinueSimulation()
        {
            if(_simulationState.State == SimulationStates.SimulationPaused)
                _simulationState.State = SimulationStates.Waiting;
            Timer.Start();
        }

        #endregion

        #region Protectes Methods

        /// <summary>
        /// <para>If the simulation have been ended set the initial position.</para>
        /// <para>Initialises the controller if it's before the first step.</para>
        /// </summary>
        protected void InitSimulationIfNeeded()
        {
            if (_simulationState.State == SimulationStates.SimulationEnded)
            {
                SetInitialPosition();
            }

            if (_simulationData.Step == 0)
            {
                var taskDistributor = _serviceLocator.GetTaskDistributor(_simulationData);

                _simulationState.State = SimulationStates.ControllerWorking;
                _controller.InitializeController(_simulationData, TimeSpan.FromSeconds(6), taskDistributor);
            }
        }

        #endregion
    }
}
