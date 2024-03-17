using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Interfaces
{
    public interface IController
    {
        /// <summary>
        /// Before starting the simulation it should be called.
        /// </summary>
        /// <param name="timeSpan">Time span to initialise the controller</param>
        /// <param name="simulationData">The data of the simulation</param>
        public void InitializeController(TimeSpan timeSpan, SimulationData simulationData);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSpan">Time span to return the calculated operations</param>
        /// <returns>Returns the next RobotOperation for every Robot</returns>
        public RobotOperation[] ClaculateOperations(TimeSpan timeSpan);

    }
}
