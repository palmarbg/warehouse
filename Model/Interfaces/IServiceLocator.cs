using Persistence.DataTypes;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IServiceLocator
    {
        IDataAccess GetConfigDataAccess(string path);
        IController GetController();
        IExecutor GetExecutor(SimulationData simulationData);
        ILoadLogDataAccess GetLoadLogDataAccess(string path, IDataAccess dataAccess);
        IController GetReplayController(ILoadLogDataAccess loadLogDataAccess);
        IExecutor GetReplayExecutor(SimulationData simulationData);
        IReplayMediator GetReplayMediator(Simulation simulation);
        ITaskDistributor GetReplayTaskDistributor(SimulationData simulationData);
        ISimulationMediator GetSimulationMediator(Simulation simulation);
        ITaskDistributor GetTaskDistributor(SimulationData simulationData);
    }
}
