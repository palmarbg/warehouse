using RobotokModel.Model.Interfaces;
using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Mediators
{
    public abstract class AbstractMediator : IMediator
    {
        #region Protected Fields

        protected readonly System.Timers.Timer Timer;

        protected IDataAccess dataAccess = null!;
        protected ITaskDistributor taskDistributor = null!;
        protected IController controller = null!;
        protected IExecutor executor = null!;
        

        protected int interval;

        protected SimulationState simulationState;
        protected SimulationData simulationData = null!;

        protected readonly Simulation simulation;

        #endregion

        #region Properties

        public SimulationData SimulationData => simulationData;
        public SimulationState SimulationState => simulationState;
        public int Interval => interval;

        #endregion

        #region Constructor

        public AbstractMediator(Simulation simulation)
        {
            this.simulation = simulation;

            interval = 100;
            Timer = new System.Timers.Timer
            {
                Interval = interval,
                Enabled = true
            };
            Timer.Stop();

            simulationState = new SimulationState();
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

            SetInitialState();

            simulationState.IsSimulationRunning = true;
            simulationState.IsSimulationEnded = false;

            taskDistributor.TaskAssigned += new EventHandler<(Robot, Goal)>((_, robotAndGoal) => OnTaskAssigned(robotAndGoal.Item1.Id, robotAndGoal.Item2.Id));

            controller.FinishedTask += new EventHandler<IControllerEventArgs>((sender, e) =>
            {
                if (controller != sender)
                    return;
                OnTaskFinished(e);
            });

            controller.InitializeController(simulationData, TimeSpan.FromSeconds(6), taskDistributor);

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

            simulationState = new SimulationState();

            simulationData = dataAccess.GetInitialSimulationData();
            taskDistributor = taskDistributor.NewInstance(simulationData);
            controller = controller.NewInstance();
            executor = executor.NewInstance(simulationData);

            simulation.OnSimulationLoaded();

        }

        public void LoadSimulation(string filePath)
        {
            dataAccess = dataAccess.NewInstance(filePath);
            SetInitialState();
        }

        #endregion

        #region Private methods

        private void OnTaskFinished(IControllerEventArgs e)
        {
            Debug.WriteLine("TASK FINISHED");

            simulationState.IsExecutingMoves = true;
            simulationState.IsLastTaskFinished = true;

            executor.ExecuteOperations(e.robotOperations);
            simulationState.IsExecutingMoves = false;

            simulationData.Step++;

            simulation.OnRobotsMoved();
        }

        #endregion

        #region Protected methods

        protected virtual void OnTaskAssigned(int robotId, int taskId)
        {
            
        }

        #endregion
    }
}
