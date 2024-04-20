using Persistence.DataTypes;

namespace RobotokModel.Persistence.Interfaces
{
    public interface ILoadLogDataAccess : IDataAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="step">Index of step</param>
        /// <returns>The actual operations executed</returns>
        RobotOperation[] GetRobotOperations(int step);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Task events of each robot</returns>
        List<TaskEvent[]> GetTaskEvents();
    }
}
