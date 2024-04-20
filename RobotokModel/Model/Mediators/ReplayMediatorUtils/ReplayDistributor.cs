using Persistence.DataTypes;
using RobotokModel.Model.Interfaces;

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
