using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Interfaces
{
    public interface IMediator
    {
        #region Properties

        public SimulationData SimulationData { get; }
        public IDataAccess DataAccess { init; }
        public ITaskDistributor TaskDistributor { init; }
        public IController Controller { init; }
        public IExecutor Executor { init; }
        public ILogger Logger { init; }

        #endregion

        void StartSimulation();
        void StopSimulation();
        void SetController(string name);
        void SetTaskDistributor(string name);
        void SetInitialPosition();
        void LoadSimulation(string filePath);

        //IMediator NewInstance();
    }
}
