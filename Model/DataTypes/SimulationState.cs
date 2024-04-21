namespace Model.DataTypes
{

    public class SimulationState
    {
        public event EventHandler<SimulationState>? SimulationStateChanged;
        private void OnSimulationStateChanged()
        {
            SimulationStateChanged?.Invoke(this, this);
        }

        public void Reset()
        {
            _isSimulationRunning = false;
            _isSimulationEnded = true;
            IsLastTaskFinished = true;
            IsExecutingMoves = false;
            OnSimulationStateChanged();
        }

        private bool _isSimulationRunning = false;
        private bool _isSimulationEnded = true;

        public bool IsSimulationRunning
        {
            get => _isSimulationRunning;
            set
            {
                _isSimulationRunning = value;
                OnSimulationStateChanged();
            }
        }

        public bool IsSimulationEnded
        {
            get => _isSimulationEnded;
            set
            {
                _isSimulationEnded = value;
                OnSimulationStateChanged();
            }
        }

        public bool IsLastTaskFinished { get; set; } = true;
        public bool IsExecutingMoves { get; set; } = false;
        public bool IsSimulationPaused => !IsSimulationRunning && !IsSimulationEnded;
        public bool IsSimulationStarted => IsSimulationRunning || !IsSimulationEnded;
    }

}
