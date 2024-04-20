using Persistence.DataTypes;

namespace RobotokModel.Interfaces
{
    public interface IController
    {
        /// <summary>
        /// Returns the name of the controller
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Before starting the simulation it should be called.
        /// </summary>
        /// <param name="timeSpan">Time span to initialise the controller</param>
        /// <param name="simulationData">The data of the simulation</param>
        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan, ITaskDistributor distributor);

        /// <summary>
        /// Sets the next operation for every robot.
        /// Async method
        /// </summary>
        /// <param name="timeSpan">Time span to return the calculated operations</param>
        /// <returns>Returns the next RobotOperation for every Robot</returns>
        public void CalculateOperations(TimeSpan timeSpan);

        /// <summary>
        /// This will be invoked when the controller finishes the task
        /// </summary>
        public event EventHandler<IControllerEventArgs> FinishedTask;

        public IController NewInstance();

    }

    public class IControllerEventArgs(RobotOperation[] robotOperations) : EventArgs
    {
        public RobotOperation[] robotOperations { get; set; } = robotOperations;
    }
}
