using RobotokModel.Model.Interfaces;
using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Mediators.ReplayMediatorUtils
{
    public class ReplayDistributor : ITaskDistributor
    {
        public bool AllTasksAssigned => throw new NotImplementedException();
        public event EventHandler<(Robot, Goal)>? TaskAssigned;

        public void AssignNewTask(Robot robot)
        {
            throw new NotImplementedException();
        }

        public ITaskDistributor NewInstance(SimulationData simulationData)
        {
            return this;
        }
    }
}
