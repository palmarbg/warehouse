using Model.DataTypes;
using Persistence.DataTypes;

namespace Model.Interfaces
{
    public interface IMediator
    {
        #region Properties

        public SimulationData SimulationData { get; }
        public SimulationState SimulationState { get; }
        public int Interval { get; }

        public string MapFileName { get; }

        #endregion

        #region Methods

        void StartSimulation();
        void StopSimulation();
        void PauseSimulation();
        void SetInitialState();

        #endregion
    }
}
