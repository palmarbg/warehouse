
namespace Persistence.DataTypes
{
    public enum Direction
    {
        Left, Up, Right, Down
    }

    public enum RobotOperation
    {
        Forward, Clockwise, CounterClockwise, Backward, Wait, Timeout
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
}
