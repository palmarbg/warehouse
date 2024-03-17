using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Interfaces
{
    /// <summary>
    /// Constructor should get SimulationData
    /// </summary>
    public interface ITaskDistributor
    {
        /// <summary>
        /// Assign new task to <c>robot</c>
        /// </summary>
        /// <param name="robot">The robot, that should get a new goal</param>
        public void AssignNewTask(Robot robot);

    }
}
