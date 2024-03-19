using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Interfaces
{
    public interface IController
    {
        /// <summary>
        /// Before starting the simulation it should be called.
        /// </summary>
        /// <param name="timeSpan">Time span to initialise the controller</param>
        /// <param name="simulationData">The data of the simulation</param>
        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan);

        /// <summary>
        /// Sets the next operation to every robot.
        /// This method may be async
        /// </summary>
        /// <param name="timeSpan">Time span to return the calculated operations</param>
        /// <returns>Returns the next RobotOperation for every Robot</returns>
        public void ClaculateOperations(TimeSpan timeSpan);

        /// <summary>
        /// This will be invoked when the controller finishes the task
        /// </summary>
        public event EventHandler<IControllerEventArgs> FinishedTask;

    }

    public class IControllerEventArgs : EventArgs
    {
        public RobotOperation[] robotOperations { get; set; }

        public IControllerEventArgs(RobotOperation[] robotOperations)
        {
            this.robotOperations = robotOperations;
        }
    }
}
