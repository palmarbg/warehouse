
namespace Model.DataTypes
{
    public class SimulationStateEventArgs : EventArgs
    {
        public required SimulationState SimulationState { get; init; }
        public required bool IsReplayMode { get; init; }
    }
}
