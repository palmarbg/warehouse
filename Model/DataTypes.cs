namespace Model
{

    public class SimulationState
    {
        public bool IsSimulationRunning { get; set; } = false;
        public bool IsSimulationEnded { get; set; } = true;
        public bool IsLastTaskFinished { get; set; } = true;
        public bool IsExecutingMoves { get; set; } = false;
        public bool IsSimulationPaused => !IsSimulationRunning && !IsSimulationEnded;
    }

    //public class SimulationEvents
    //{
    //    public event EventHandler? ControllerTimeout;
    //    public event EventHandler? ControllerTaskFinished;
    //    public event EventHandler? ExecutorFinished;
    //    public event EventHandler? TaskFinished;
    //    //public event EventHandler? TaskAssigned;
    //}

}
