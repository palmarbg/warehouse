using Persistence.DataTypes;

namespace Model.Interfaces
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

        public event EventHandler<(Robot, Goal)>? TaskAssigned;

        public bool AllTasksAssigned { get; }

        public ITaskDistributor NewInstance(SimulationData simulationData);


    }
}
