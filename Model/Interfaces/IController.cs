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
        /// It will be invoked when the controller finishes the task
        /// </summary>
        public event EventHandler<IControllerEventArgs> FinishedTask;

        /// <summary>
        /// It will be invoked when the controller finishes the initialization
        /// </summary>
        public event EventHandler InitializationFinished;

        /// <summary>
        /// Initializes the controller before the first step. When finishes, <see cref="InitializationFinished"/> will be invoked.
        /// </summary>
        /// <param name="simulationData">The data of the simulation</param>
        /// <param name="timeSpan">Time span to initialise the controller</param>
        /// <param name="distributor">The distributor used to get new goals for robots</param>
        /// <param name="token">Cancellation token, if the simulation requests a stop</param>
        /// <remarks>
        /// It should be called before starting the simulation.
        /// </remarks>
        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan, ITaskDistributor distributor, CancellationToken? token = null);

        /// <summary>
        /// Sets the next operation for every robot. When finishes, <see cref="FinishedTask"/> will be invoked.
        /// </summary>
        /// <param name="timeSpan">Time span given for calculating the operations</param>
        /// <param name="token">Cancellation token, if the simulation requests a stop</param>
        public void CalculateOperations(TimeSpan timeSpan, CancellationToken? token = null);

        /// <summary>
        /// Returns a new instance of the same class.
        /// </summary>
        public IController NewInstance();

    }
}
