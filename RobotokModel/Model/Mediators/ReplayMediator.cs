using RobotokModel.Model.Controllers;
using RobotokModel.Model.Distributors;
using RobotokModel.Model.Executors;
using RobotokModel.Model.Interfaces;
using RobotokModel.Model.Mediators.ReplayMediatorUtils;
using RobotokModel.Persistence.DataAccesses;
using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Mediators
{
    public class ReplayMediator : AbstractMediator, IReplayMediator
    {
        #region Constructor

        public ReplayMediator(Simulation simulation) : base(simulation)
        {

            Timer.Elapsed += (_, _) => StepSimulation();

            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("Robotok"));
            IDataAccess mapDataAccess = new ConfigDataAccess(path + "sample_files\\random_20_config.json");
            //dataAccess = new MockLoadLogDataAccess();
            dataAccess = new LoadLogDataAccess(path + "sample_files\\random_20_log.json", mapDataAccess);

            simulationData = dataAccess.GetInitialSimulationData();

            controller = new ReplayController((ILoadLogDataAccess)dataAccess);
            taskDistributor = new ReplayDistributor();
            executor = new ReplayExecutor(simulationData);
            controller.InitializeController(simulationData, TimeSpan.FromSeconds(6), taskDistributor);

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
        }

        #endregion
    }
}
