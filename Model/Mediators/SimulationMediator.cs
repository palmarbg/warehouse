using Model.Interfaces;
using System.Diagnostics;
using System.Timers;

namespace Model.Mediators
{
    public class SimulationMediator : AbstractMediator, ISimulationMediator
    {
        #region Constructor

        public SimulationMediator(ISimulation simulation, IServiceLocator serviceLocator, string mapFileName) : base(simulation, serviceLocator, mapFileName)
        {

            Timer.Elapsed += (_, _) => StepSimulation();

            dataAccess = serviceLocator.GetConfigDataAccess(mapFileName);

            simulationData = dataAccess.GetInitialSimulationData();

            controller = serviceLocator.GetController();

            executor = _serviceLocator.GetExecutor(simulationData);

        }

        #endregion

        #region Public methods

        public void LoadConfig(string fileName)
        {
            MapFileName = fileName;
            dataAccess = dataAccess.NewInstance(fileName);
            SetInitialState();
        }

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
