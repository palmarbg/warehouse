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
        public int X {  get; set; }
        public int Y { get; set; }
    }

    public struct RobotMove
    {
        Position Position;
        char Operation;
    }

    public struct SimulationData
    {
        public ITile[][] Map;
    }

}
