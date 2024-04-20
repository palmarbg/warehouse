using Persistence.DataTypes;

namespace RobotokModel.Interfaces
{
    public interface IMediator
    {
        #region Properties

        public SimulationData SimulationData { get; }
        public SimulationState SimulationState { get; }
        public int Interval { get; }

        #endregion


        #region Methods

        void StartSimulation();
        void StopSimulation();
        void PauseSimulation();
        void SetInitialState();

        /// <summary>
        /// Start new simulation from file
        /// </summary>
        /// <param name="filePath">Absolute path for file</param>
        void LoadSimulation(string filePath);

        #endregion
    }
}
