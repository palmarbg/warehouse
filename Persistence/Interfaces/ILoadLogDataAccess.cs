using Persistence.DataTypes;

namespace Persistence.Interfaces
{
    /// <summary>
    /// File access for loading log
    /// </summary>
    public interface ILoadLogDataAccess : IDataAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="step">Index of step</param>
        /// <returns>The actual operations executed in the <paramref name="step"/>-th step.</returns>
        RobotOperation[] GetRobotOperations(int step);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Task events of each robot</returns>
        List<TaskEvent[]> GetTaskEvents();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The total number of steps in the replay.</returns>
        int GetStepCount();
    }
}
