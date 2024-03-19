using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Interfaces
{
    /// <summary>
    /// Constructor will get SimulationData reference
    /// </summary>
    public interface IExecutor
    {
        /// <summary>
        /// All robot will wait
        /// </summary>
        public void Timeout();

        /// <summary>
        /// Update robot positions
        /// </summary>
        /// <param name="robotOperations"></param>
        public void ExecuteOperations(RobotOperation[] robotOperations);
    }
}
