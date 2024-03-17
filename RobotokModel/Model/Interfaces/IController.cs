using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Interfaces
{
    public interface IController
    {
        public SimulationData SimulationData { get; set; }

        /// <summary>
        /// Updates robots NextOperation property
        /// </summary>
        /// <returns>
        /// Robotoperation[] for Logging
        /// </returns>
        public RobotOperation[] NextStep();
        public RobotOperation[] ClaculateOperations();
    }
}
