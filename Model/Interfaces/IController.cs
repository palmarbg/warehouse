using Model.DataTypes;
using Persistence.DataTypes;

namespace Model.Interfaces
{
    public interface IController
    {
        /// <summary>
        /// Returns the name of the controller
        /// </summary>
        string Name { get; }

        /// <summary>
        /// This will be invoked when the controller finishes the task
        /// </summary>
        public event EventHandler<IControllerEventArgs> FinishedTask;

        /// <summary>
        /// It will be invoked when the controller finishes the initialization
        /// </summary>
        public event EventHandler InitializationFinished;

        /// <summary>
        /// Before starting the simulation it should be called.
        /// </summary>
        /// <param name="timeSpan">Time span to initialise the controller</param>
        /// <param name="simulationData">The data of the simulation</param>
        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan, ITaskDistributor distributor, CancellationToken? token = null);

        /// <summary>
        /// Sets the next operation for every robot.
        /// </summary>
        /// <param name="timeSpan">Time span to return the calculated operations</param>
        /// <returns>Returns the next RobotOperation for every Robot</returns>
        public void CalculateOperations(TimeSpan timeSpan, CancellationToken? token = null);

        public IController NewInstance();

    }
}
