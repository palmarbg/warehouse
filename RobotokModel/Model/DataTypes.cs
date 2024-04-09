using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model
{
    public enum Direction
    {
        Left, Up, Right, Down
    }

    public enum RobotOperation
    {
        Forward, Clockwise, CounterClockwise, Backward, Wait
    }

    public struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override readonly string ToString()
        {
            return $"({X},{Y})";
        }

    }

    public struct RobotMove
    {
        public Position Position;
        public char Operation;
    }

    public class SimulationData
    {
        public required ITile[,] Map;
        public required List<Goal> Goals;
        public required List<Robot> Robots;
        public required string DistributorName;
        public required int RevealedTaskCount;
        //public int Step { get; set; } = 0;
    }

    public class Goal
    {
        #region Static

        public static event EventHandler? GoalsChanged;
        public static void OnGoalsChanged()
        {
            GoalsChanged?.Invoke(null, new());
        }

        #endregion
        public required int Id { get; init; }
        public required Position Position { get; set; }
        public bool IsAssigned { get; set; } = false;

        public Goal() { }

    }

    public class SimulationState
    {
        public bool IsSimulationRunning { get; set; } = false;
        public bool IsSimulationEnded { get; set; } = true;
        public bool IsLastTaskFinished { get; set; } = true;
        public bool IsExecutingMoves { get; set; } = false;
        public bool IsSimulationPaused => !IsSimulationRunning && !IsSimulationEnded;
    }

}
