using RobotokModel.Model;
using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence.DataAccesses
{
    public class LoadLogDataAccess : ILoadLogDataAccess
    {
        //do it just like ConfigDataAccess :)

        #region Private fields

        private Uri baseUri;
        private string path;
        private SimulationData simulationData = null!;

        #endregion

        #region Constructor
        public LoadLogDataAccess(string path)
        {
            this.path = path;
            baseUri = new(path);
        }

        #endregion

        #region Public methods
        public SimulationData GetInitialSimulationData()
        {
            throw new NotImplementedException();
        }

        public RobotOperation[] GetRobotOperations(int step)
        {
            throw new NotImplementedException();
        }

        public IDataAccess NewInstance(string filePath)
        {
            return new DemoDataAccess(filePath);
        }

        #endregion

        #region Private methods

        #endregion
    }
}
