using Model.DataTypes;
using Persistence.DataTypes;

namespace Model.Interfaces
{
    public interface ISimulation : IReplayMediator, ISimulationMediator
    {
        /// <summary>
        /// Indicates whether the simulation is simulated or replayed.
        /// </summary>
        public bool IsInSimulationMode { get; }

        #region Events

        /// <summary>
        /// Occurs when robots have moved.
        /// </summary>
        public event EventHandler<RobotsMovedEventArgs>? RobotsMoved;

        /// <summary>
        /// Occurs when a goal have been assigned or completed.
        /// </summary>
        public event EventHandler<Goal?>? GoalChanged;

        /// <summary>
        /// Occurs when the simulation ended.
        /// </summary>
        public event EventHandler? SimulationFinished;

        /// <summary>
        /// Occurs when <see cref="SimulationData"/> has been loaded for the simulation.
        /// </summary>
        public event EventHandler? SimulationLoaded;

        /// <summary>
        /// Occurs when <see cref="SimulationState"/> has changed.
        /// </summary>
        public event EventHandler<SimulationStateEventArgs>? SimulationStateChanged;

        #endregion

        #region Event Methods

        void OnSimulationStateChanged(SimulationState simulationState);

        void OnSimulationLoaded();

        void OnRobotsMoved(RobotsMovedEventArgs args);

        void OnSimulationFinished();

        #endregion

        /// <summary>
        /// Starts a new simulation in simulation mode with the current config file.
        /// </summary>
        void StartNewSimulation();

    }
}
