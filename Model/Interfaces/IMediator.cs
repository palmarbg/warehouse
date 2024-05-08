using Model.DataTypes;
using Persistence.DataTypes;

namespace Model.Interfaces
{
    public interface IMediator : IDisposable
    {
        #region Properties

        /// <summary>
        /// Represents the current state of the warehouse, including robots and goals.
        /// </summary>
        public SimulationData SimulationData { get; }

        /// <summary>
        /// Describes the current state of the Mediator.
        /// </summary>
        public SimulationState SimulationState { get; }

        /// <summary>
        /// The time span in milliseconds, the Controller has to calculate moves.
        /// </summary>
        public int Interval { get; }

        /// <summary>
        /// The name of the currently loaded config file.
        /// </summary>
        public string MapFileName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Starts a new simulation if the previous one ended.
        /// Else continues the previous one.
        /// </summary>
        void StartSimulation();

        /// <summary>
        /// Stops the simulation immediately, and marks it as ended.
        /// </summary>
        void StopSimulation();

        /// <summary>
        /// Suspends the simulation.
        /// </summary>
        /// <remarks>
        /// If the <see cref="SimulationState"/> is <see cref="SimulationStates.ControllerWorking"/> or
        /// <see cref="SimulationStates.ExecutingMoves"/>, then the simulation will be paused,
        /// when the <see cref="IExecutor"/> finished executing.
        /// </remarks>
        void PauseSimulation();

        /// <summary>
        /// Stops the simulation immediately.
        /// Reloads the map, creates new <see cref="IController"/> and <see cref="IExecutor"/>.
        /// </summary>
        void SetInitialPosition();

        #endregion
    }
}
