
using Persistence.DataTypes;

namespace Model.DataTypes
{
    public class SimulationStateEventArgs : EventArgs
    {
        public required SimulationState SimulationState { get; init; }
        public required bool IsReplayMode { get; init; }
    }

    public class RobotsMovedEventArgs : EventArgs
    {
        public required TimeSpan TimeSpan { get; init; }

        /// <summary>
        /// If it's not a single step, but a jump.
        /// </summary>
        public required bool IsJumped { get; init; }
    }

    public class ControllerEventArgs(RobotOperation[] _robotOperations) : EventArgs
    {
        public RobotOperation[] robotOperations => _robotOperations;
    }

    public class SimulationStepEventArgs(int step) : EventArgs
    {
        public int Step => step;
    }
}
