using System;
using System.Collections.Generic;
using System.Linq;
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

    public struct SimulationData
    {
        public ITile[,] Map;
        public List<Goal> Goals;
        
    }

    public struct Goal
    {
        private static int id = 0;
        public int Id { get; }
        public Position Position { get; set; }
        public Goal()
        {
            Id = id++;
        }
    }

}
