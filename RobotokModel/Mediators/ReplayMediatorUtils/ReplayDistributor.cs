using Persistence.DataTypes;
using RobotokModel.Interfaces;

namespace RobotokModel.Mediators.ReplayMediatorUtils
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
