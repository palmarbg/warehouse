using Persistence.DataTypes;

namespace Persistence.Interfaces
{
    /// <summary>
    /// File access for saving log
    /// </summary>
    public interface ISaveLogDataAccess
    {
        /// <summary>
        /// Save log data to file
        /// </summary>
        /// <param name="path">absolute path for saving</param>
        /// <param name="log">content to save</param>
        void SaveLogData(string path, Log log);

        /// <summary>
        /// Returns a new instance of the same class.
        /// </summary>
        ISaveLogDataAccess NewInstance();
    }
}
