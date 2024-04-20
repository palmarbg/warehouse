using Model.Controllers;
using Model.Distributors;
using Model.Executors;
using Model.Interfaces;
using Persistence.DataAccesses;
using Persistence.Interfaces;
using Persistence.Loggers;
using System.Diagnostics;
using System.Timers;

namespace Model.Mediators
{
    public class SimulationMediator : AbstractMediator, ISimulationMediator
    {
        #region Constructor

        public SimulationMediator(Simulation simulation, IServiceLocator serviceLocator) : base(simulation, serviceLocator)
        {

            Timer.Elapsed += (_, _) => StepSimulation();

            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("View"));

            dataAccess = serviceLocator.GetConfigDataAccess(path + "sample_files\\astar1test.json");

            simulationData = dataAccess.GetInitialSimulationData();

            controller = serviceLocator.GetController();

            executor = _serviceLocator.GetExecutor(simulationData);

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
    }
}
