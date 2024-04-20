using RobotokModel.Controllers;
using RobotokModel.Distributors;
using RobotokModel.Executors;
using RobotokModel.Interfaces;
using Persistence.DataAccesses;
using Persistence.Interfaces;
using Persistence.Loggers;
using System.Diagnostics;
using System.Timers;

namespace RobotokModel.Mediators
{
    public class SimulationMediator : AbstractMediator, ISimulationMediator
    {
        #region Constructor

        public SimulationMediator(Simulation simulation) : base(simulation)
        {

            Timer.Elapsed += (_, _) => StepSimulation();

            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("Robotok"));
            dataAccess = new ConfigDataAccess(path + "sample_files\\astar1test.json");

            simulationData = dataAccess.GetInitialSimulationData();

            controller = new AStarController();
            taskDistributor = new DemoDistributor(simulationData);

            ILogger logger = new BasicLogger(simulationData);
            executor = new DefaultExecutor(simulationData, logger);

            controller.InitializeController(simulationData, TimeSpan.FromSeconds(6), taskDistributor);

        }

        #endregion

        #region Public methods

        public void SaveSimulation(string filepath)
        {
            executor.SaveSimulation(filepath);
        }

        #endregion

        #region Private methods

        private void StepSimulation()
        {
            Debug.WriteLine("--SIMULATION STEP--");

            if (simulationState.IsExecutingMoves)
                return;

            if (!simulationState.IsLastTaskFinished)
            {
                OnTaskTimeout();
                return;
            }

            simulationState.IsLastTaskFinished = false;
            time = DateTime.Now;
            controller.CalculateOperations(TimeSpan.FromMilliseconds(interval));

        }

        private void OnTaskTimeout()
        {
            Debug.WriteLine("XXXX TIMEOUT XXXX");
            executor.Timeout();
            simulationData.Step++;
        }

        #endregion

        #region Protected methods

        protected override void OnTaskAssigned(int robotId, int taskId)
        {
            executor.TaskAssigned(taskId, robotId);
        }

        #endregion
    }
}
