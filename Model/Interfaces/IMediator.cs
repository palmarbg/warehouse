using Model.DataTypes;
using Persistence.DataTypes;

namespace Model.Interfaces
{
    public interface IMediator
    {
        #region Properties

        /// <summary>
        /// The current state of the warehouse, robots and goals
        /// </summary>
        public SimulationData SimulationData { get; }

        /// <summary>
        /// Describes the current state of the Mediator.
        /// </summary>
        public SimulationState SimulationState { get; }

        /// <summary>
        /// The timespan in milliseconds, the Controller has to calculate moves.
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
        /// Stops the simulation, and marks it as ended.
        /// </summary>
        void StopSimulation();

        /// <summary>
        /// Suspends the simulation, and marks it as paused.
        /// </summary>
        void PauseSimulation();

        /// <summary>
        /// Stops the simulation.
        /// Reloads the map, resets the Controller and Executor.
        /// </summary>
        void SetInitialPosition();

        #endregion
    }
}
