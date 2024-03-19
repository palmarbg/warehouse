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
        public List<Goal> Goals = [];
        public int Step { get; set; } = 0;
    }

    public struct Goal
    {
        #region Static

        private static int id = 0;
        public static event EventHandler? GoalsChanged;

        public static void OnGoalsChanged()
        {
            GoalsChanged?.Invoke(null, new());
        }

        #endregion

        public int Id { get; }
        public Position Position { get; set; }
        public bool IsAssigned { get; set; }
        public Goal()
        {
            Id = id++;
            IsAssigned = false;
        }
    }

}
