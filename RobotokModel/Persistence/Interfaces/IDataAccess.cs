using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence.Interfaces
{
    /// <summary>
    /// Config file access
    /// </summary>
    public interface IDataAccess
    {
        //Constructor will get the path for the file

        /// <returns>The initial state of the simulation loaded from the config file</returns>
        SimulationData GetInitialSimulationData();
    }
}
