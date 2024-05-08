using Model.Controllers;
using Model.Distributors;
using Model.Executors;
using Model.Interfaces;
using Model.Mediators;
using Model.Utils.ReplayMediatorUtils;
using Model.Utils;
using Persistence.DataAccesses;
using Persistence.DataTypes;
using Persistence.Interfaces;
using Persistence.Loggers;

namespace Model
{

    public record struct ServiceLocator : IServiceLocator
    {
        #region Mediators

        public ISimulationMediator GetSimulationMediator(Simulation simulation, string mapFileName)
        {
            return new SimulationMediator(simulation, this, mapFileName);
        }

        public IReplayMediator GetReplayMediator(Simulation simulation, string mapFileName, string logFileName)
        {
            return new ReplayMediator(simulation, this, mapFileName, logFileName);
        }

        #endregion

        #region DataAccess

        public IDataAccess GetConfigDataAccess(string path)
        {
            return new ConfigDataAccess(path, GetDirectoryDataAccess());
        }

        public ILoadLogDataAccess GetLoadLogDataAccess(string path, IDataAccess dataAccess)
        {
            return new LoadLogDataAccess(path, dataAccess);
        }

        #endregion

        #region Simulation controlls

        public IController GetController()
        {
            return new DisposableController(new CooperativeAStarController());
        }

        public ITaskDistributor GetTaskDistributor(SimulationData simulationData)
        {
            return new DemoDistributor(simulationData);
        }

        public IExecutor GetExecutor(SimulationData simulationData)
        {
            return new DefaultExecutor(simulationData, GetLogger(simulationData));
        }

        #endregion

        #region Replay

        public IController GetReplayController(ILoadLogDataAccess loadLogDataAccess)
        {
            return new ReplayController(loadLogDataAccess);
        }

        public ITaskDistributor GetReplayTaskDistributor(SimulationData simulationData)
        {
            return new DemoDistributor(simulationData);
        }

        public IExecutor GetReplayExecutor(SimulationData simulationData)
        {
            return new ReplayExecutor(simulationData);
        }

        #endregion

        #region Private methods

        private ILogger GetLogger(SimulationData simulationData)
        {
            return new BasicLogger(simulationData, GetSaveLogDataAccess());
        }

        private ISaveLogDataAccess GetSaveLogDataAccess()
        {
            return new SaveLogDataAccess();
        }

        private IDirectoryDataAccess GetDirectoryDataAccess()
        {
            return new DirectoryDataAccess();
        }

        #endregion
    }
}
