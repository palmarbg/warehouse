using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence.Interfaces
{
    //TODO: TASK LOGGING IS AMBIGOUS: [TASKID, FROM<-???, TO]

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

        /// <summary>
        /// Logs a simulation step. Called by the Executor
        /// </summary>
        /// <param name="controllerOperations">Operations given by the Controller</param>
        /// <param name="robotOperations">Operations executed by the Executor</param>
        /// <param name="errors">Operation errors occurred during execution</param>
        void LogStep(
            RobotOperation[] controllerOperations,
            RobotOperation[] robotOperations,
            OperationError[] errors
            );

        /// <summary>
        /// Logs when Controller timeout occurs. Called by the Executor
        /// </summary>
        void LogTimeout();

        /// <summary>
        /// Logs when a task is assigned or completed. Called by the Distributor or Executor
        /// </summary>
        /// <param name="taskEvent"></param>
        void LogEvent(TaskEvent taskEvent);
    }
}
