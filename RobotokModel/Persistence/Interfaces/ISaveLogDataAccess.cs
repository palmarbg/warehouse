using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence.Interfaces
{
    public interface ISaveLogDataAccess
    {
        /// <summary>
        /// Save log data to file
        /// </summary>
        /// <param name="path">absolute path for saving</param>
        /// <param name="log">content to save</param>
        void SaveLogData(string path, Log log);
    }
}
