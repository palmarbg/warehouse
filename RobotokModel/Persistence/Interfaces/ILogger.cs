using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence.Interfaces
{
    /// <summary>
    /// Creates Log and saves.
    /// <para/>
    /// Constructor will get the initial position
    /// </summary>
    public interface ILogger
    {

        /// <summary>
        /// Saves the log into the file specified by <c>path</c>
        /// </summary>
        /// <param name="path">The file, where the log will be saved</param>
        /// <returns></returns>
        Task SaveLog(string path); //alternative: void SaveLog(string path);


    }
}
