using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model
{
    internal interface IController
    {
        public SimulationData SimulationData { get; set; }

        /// <summary>
        /// returns Robotoperation[] for Logging
        /// robots NextOperation prop is updated
        /// </summary>
        /// <returns></returns>
        public RobotOperation[] NextStep();
        public RobotOperation[] ClaculateOperations();
    }
}
