using Persistence.DataTypes;

namespace Model.Interfaces
{
    /// <summary>
    /// Executes moves/operations on Robots.
    /// </summary>
    public interface IExecutor : IDisposable
    {
        /// <summary>
        /// Called when <see cref="IController"/> doesn't return any operations in time.
        /// </summary>
        /// <remarks>
        /// All robot will wait.
        /// </remarks>
        public void Timeout();

        /// <summary>
        /// Updates robot positions.
        /// </summary>
        /// <param name="robotOperations">Robot operations returned by <see cref="IController"/>.</param>
        /// <param name="timeSpan">Time in milliseconds, that <see cref="IController"/> took to calculate operations.</param>
        /// <returns>
        /// Robot operations that were actually executed.
        /// </returns>
        /// <remarks>
        /// Each <see cref="IExecutor"/> might have a different set of rules to execute operations.
        /// </remarks>
        public RobotOperation[] ExecuteOperations(RobotOperation[] robotOperations, float timeSpan);

        /// <summary>
        /// Saves the log of the last simulation played.
        /// </summary>
        /// <param name="filepath">The absolute file path where to save the simulation log.</param>
        public void SaveSimulationLog(string filepath);

        /// <summary>
        /// Returns a new instance of the same class.
        /// </summary>
        /// <param name="simulationData">Simulation data to be operated on.</param>
        public IExecutor NewInstance(SimulationData simulationData);

    }
}
