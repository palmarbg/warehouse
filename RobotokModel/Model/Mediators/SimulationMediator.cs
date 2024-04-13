using RobotokModel.Model.Controllers;
using RobotokModel.Model.Distributors;
using RobotokModel.Model.Executors;
using RobotokModel.Model.Interfaces;
using RobotokModel.Persistence.DataAccesses;
using RobotokModel.Persistence.Interfaces;
using RobotokModel.Persistence.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RobotokModel.Model.Mediators
{
    internal class SimulationMediator : AbstractMediator, ISimulationMediator
    {
        #region Private fields

        protected ILogger logger = null!;

        #endregion

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
            //save
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
            controller.CalculateOperations(TimeSpan.FromMilliseconds(interval));

        }

        private void OnTaskTimeout()
        {
            Debug.WriteLine("XXXX TIMEOUT XXXX");
            executor.Timeout();
            simulationData.Step++;
        }

        #endregion
    }
}
