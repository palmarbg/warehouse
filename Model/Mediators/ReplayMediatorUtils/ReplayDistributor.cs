using Model.Interfaces;
using Persistence.DataTypes;

namespace Model.Mediators.ReplayMediatorUtils
{
    public class ReplayDistributor : ITaskDistributor
    {
        public bool AllTasksAssigned => throw new NotImplementedException();

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
