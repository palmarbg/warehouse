using Persistence.DataTypes;

namespace Model.Interfaces
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
        /// <param name="robotOperations">Robot operations returned by the Controller</param>
        /// <returns>Robot operations that were executed</returns>
        public RobotOperation[] ExecuteOperations(RobotOperation[] robotOperations, float timeSpan);

        /// <summary>
        /// Clones the executor
        /// </summary>
        /// <param name="simulationData">Simulation data to be operated on</param>
        /// <returns>A new instance of the same executor</returns>
        public IExecutor NewInstance(SimulationData simulationData);

        public void SaveSimulation(string filepath);
        void TaskAssigned(int taskId, int robotId);

    }
}
