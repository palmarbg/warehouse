using Persistence.DataTypes;
using Persistence.Interfaces;

namespace Model.Interfaces
{
    /// <summary>
    /// Contains the current dependencies of the model.
    /// </summary>
    public interface IServiceLocator
    {
        IDataAccess GetConfigDataAccess(string path);
        IController GetController();
        IExecutor GetExecutor(SimulationData simulationData);
        ILoadLogDataAccess GetLoadLogDataAccess(string path, IDataAccess dataAccess);
        IController GetReplayController(ILoadLogDataAccess loadLogDataAccess);
        IExecutor GetReplayExecutor(SimulationData simulationData);
        IReplayMediator GetReplayMediator(Simulation simulation, string mapFileName, string logFileName);
        ISimulationMediator GetSimulationMediator(Simulation simulation, string mapFileName);
        ITaskDistributor GetTaskDistributor(SimulationData simulationData);
    }
}
