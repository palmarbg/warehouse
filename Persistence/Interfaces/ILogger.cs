using Persistence.DataTypes;

namespace Persistence.Interfaces
{
    /// <summary>
    /// Used by Executor to store and save log.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Saves the log into the file specified by <c>path</c>.
        /// </summary>
        /// <param name="path">The file, where the log will be saved.</param>
        /// <returns></returns>
        void SaveLog(string path); //alternative: void SaveLog(string path);

        /// <summary>
        /// Logs a simulation step.
        /// </summary>
        /// <param name="controllerOperations">Operations given by the Controller.</param>
        /// <param name="robotOperations">Operations executed by the Executor.</param>
        /// <param name="errors">Operation errors occurred during execution.</param>
        /// <param name="timeElapsed">The time in milliseconds the Controller spent to calculate the operations.</param>
        void LogStep(
            RobotOperation[] controllerOperations,
            RobotOperation[] robotOperations,
            OperationError[] errors,
            float timeElapsed
            );

        /// <summary>
        /// Logs when Controller timeout occurs.
        /// </summary>
        void LogTimeout();

        /// <summary>
        /// Logs when a task is assigned or completed.
        /// </summary>
        /// <param name="taskEvent">The event that occured.</param>
        /// <param name="robotId">The ID of the robot associated with the task event.</param>
        void LogEvent(TaskEvent taskEvent, int robotId);

        /// <summary>
        /// Returns a new instance of the same class.
        /// </summary>
        ILogger NewInstance(SimulationData simulationData);
    }
}
