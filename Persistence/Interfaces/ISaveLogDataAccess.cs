using Persistence.DataTypes;

namespace Persistence.Interfaces
{
    public interface ISaveLogDataAccess
    {
        /// <summary>
        /// Save log data to file
        /// </summary>
        /// <param name="path">absolute path for saving</param>
        /// <param name="log">content to save</param>
        void SaveLogData(string path, Log log);

        ISaveLogDataAccess NewInstance();
    }
}
