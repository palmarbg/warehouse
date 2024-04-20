using Model.Interfaces;
using Model.Mediators.ReplayMediatorUtils;
using Persistence.DataAccesses;
using Persistence.Interfaces;
using System.Diagnostics;

namespace Model.Mediators
{
    public class ReplayMediator : AbstractMediator, IReplayMediator
    {
        #region Constructor

        public ReplayMediator(Simulation simulation, IServiceLocator serviceLocator) : base(simulation, serviceLocator)
        {

            Timer.Elapsed += (_, _) => StepSimulation();

            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("View"));

            var mapDataAccess = serviceLocator.GetConfigDataAccess(path + "sample_files\\astar1test.json");//random_20_config
            dataAccess = _serviceLocator.GetLoadLogDataAccess(path + "sample_files\\log.json", mapDataAccess);//random_20_log

            simulationData = dataAccess.GetInitialSimulationData();

            controller = serviceLocator.GetReplayController((ILoadLogDataAccess)dataAccess);

            executor = _serviceLocator.GetReplayExecutor(simulationData);

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
