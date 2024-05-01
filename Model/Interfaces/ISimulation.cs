using Model.DataTypes;
using Persistence.DataTypes;

namespace Model.Interfaces
{
    public interface ISimulation : IReplayMediator, ISimulationMediator
    {
        #region Events

        /// <summary>
        /// When robots moved
        /// </summary>
        public event EventHandler<TimeSpan>? RobotsMoved;

        /// <summary>
        /// When goals have been added or completed
        /// </summary>
        public event EventHandler<Goal?>? GoalChanged;

        /// <summary>
        /// When simulation stops
        /// </summary>
        public event EventHandler? SimulationFinished;

        /// <summary>
        /// When map have been loaded
        /// </summary>
        public event EventHandler? SimulationLoaded;


        public event EventHandler<SimulationStateEventArgs>? SimulationStateChanged;

        #endregion

        #region Methods

        void OnSimulationStateChanged(SimulationState simulationState);

        void OnSimulationLoaded();

        void OnRobotsMoved(TimeSpan timeSpan);

        void OnSimulationFinished();

        //void LoadConfig(string fileName);

        //void LoadLog(string fileName);

        void StartNewSimulation();

        #endregion

    }
}
