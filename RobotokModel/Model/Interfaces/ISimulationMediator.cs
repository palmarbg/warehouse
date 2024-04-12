using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Interfaces
{
    public interface ISimulationMediator : IMediator
    {
        #region Properties

        public IDataAccess DataAccess { init; }
        public ITaskDistributor TaskDistributor { init; }
        public IController Controller { init; }
        public IExecutor Executor { init; }
        public ILogger Logger { init; }
        
        #endregion


    }
}
