using Persistence.DataTypes;

namespace Model.Interfaces
{
    /// <summary>
    /// Assignes new goals for robots.
    /// </summary>
    public interface ITaskDistributor
    {
        /// <summary>
        /// Assign new goal to <paramref name="robot"/>
        /// </summary>
        /// <param name="robot">The robot, that should get a new goal</param>
        public void AssignNewTask(Robot robot);

        /// <summary>
        /// Indicates wheter all tasks has been already assigned.
        /// </summary>
        public bool AllTasksAssigned { get; }

        /// <summary>
        /// Returns a new instance of the same class.
        /// </summary>
        public ITaskDistributor NewInstance(SimulationData simulationData);


    }
}
