namespace Model.DataTypes
{
    public enum SimulationStates{
        ControllerWorking,
        ExecutingMoves,
        Waiting,
        SimulationEnded,
        SimulationPaused,
    }

    public class SimulationState
    {
        private SimulationStates _simulationState = SimulationStates.SimulationEnded;

        public event EventHandler<SimulationState>? SimulationStateChanged;
        private void OnSimulationStateChanged()
        {
            SimulationStateChanged?.Invoke(this, this);
        }

        public SimulationStates State
        {
            get => _simulationState;
            set
            {
                _simulationState = value;
                OnSimulationStateChanged();
            }
        }

        public bool IsSimulationRunning =>
            _simulationState == SimulationStates.ControllerWorking ||
            _simulationState == SimulationStates.ExecutingMoves ||
            _simulationState == SimulationStates.Waiting;

    }

    class SimulationStateException : Exception
    {
        public SimulationStateException()
        {
        }

        public SimulationStateException(string? message) : base(message)
        {
        }
    }
}
