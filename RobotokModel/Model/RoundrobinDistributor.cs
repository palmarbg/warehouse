using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model
{
    internal class RoundrobinDistributor : ITaskDistributor
    {
        public SimulationData SimulationData { get ; set ; }

        public void AssignNewTask(Robot robot)
        {
            robot.CurrentGoal = SimulationData.Goals[0].Position;
            throw new NotImplementedException();
        }
    }
}
