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

        protected readonly System.Timers.Timer Timer;

        protected IDataAccess dataAccess = null!;
        protected IController controller = null!;
        protected IExecutor executor = null!;


        protected int interval;

        protected SimulationState simulationState;
        protected SimulationData simulationData = null!;

        protected readonly ISimulation simulation;

        protected DateTime time;

        protected readonly IServiceLocator _serviceLocator;

        #endregion

        #region Properties

        public SimulationData SimulationData => simulationData;
        public SimulationState SimulationState => simulationState;
        public virtual int Interval => interval;

        public string MapFileName {  get; protected set; }

        #endregion

        #region Constructor

        public AbstractMediator(ISimulation simulation, IServiceLocator serviceLocator, string mapFileName)
        {
            _serviceLocator = serviceLocator;

            this.simulation = simulation;

            interval = 500;
            Timer = new System.Timers.Timer
            {
                Interval = interval,
                Enabled = true
            };
            Timer.Stop();

            simulationState = new SimulationState();
            simulationState.SimulationStateChanged +=
                new EventHandler<SimulationState>(
                    (_, _) => simulation.OnSimulationStateChanged(simulationState)
                );
            MapFileName = mapFileName;
        }

        #endregion

        #region Public methods

        public void StartSimulation()
        {
            if (simulationState.IsSimulationRunning)
                return;

            if (simulationState.IsSimulationPaused)
            {
                simulationState.IsSimulationRunning = true;
                Timer.Start();
                return;
            }

            InitSimulation();

            simulationState.IsSimulationRunning = true;
            simulationState.IsSimulationEnded = false;

            Timer.Start();
        }

        public void StopSimulation()
        {
            if (simulationState.IsSimulationEnded)
                return;

            simulationState.IsSimulationRunning = false;
            simulationState.IsSimulationEnded = true;

            Timer.Stop();
            simulation.OnSimulationFinished();
            this.SetInitialState();
        }

        public void PauseSimulation()
        {
            if (!simulationState.IsSimulationRunning) return;
            simulationState.IsSimulationRunning = false;
            Timer.Stop();
        }

        public void SetInitialState()
        {
            Timer.Stop();

            simulationState.Reset();

            simulationData = dataAccess.GetInitialSimulationData();
            simulationData.ControllerName = controller.Name;

            controller = controller.NewInstance();
            executor = executor.NewInstance(simulationData);

            simulation.OnSimulationLoaded();

        }

        #endregion

        #region Private methods

        private void OnTaskFinished(IControllerEventArgs e)
        {
            Debug.WriteLine("TASK FINISHED");
            simulationState.IsExecutingMoves = true;
            simulationState.IsLastTaskFinished = true;

            executor.ExecuteOperations(e.robotOperations, (float)(DateTime.Now - time).TotalSeconds);
            simulationState.IsExecutingMoves = false;

            simulationData.Step++;
            Debug.WriteLine(interval);
            simulation.OnRobotsMoved(TimeSpan.FromMilliseconds(interval));
        }

        #endregion

        #region Protected methods

        protected void InitSimulation()
        {
            SetInitialState();

            controller.FinishedTask += new EventHandler<IControllerEventArgs>((sender, e) =>
            {
                if (controller != sender)
                    return;
                OnTaskFinished(e);
            });

            var taskDistributor = _serviceLocator.GetTaskDistributor(simulationData);

            controller.InitializeController(simulationData, TimeSpan.FromSeconds(6), taskDistributor);
        }

        #endregion
    }
}
