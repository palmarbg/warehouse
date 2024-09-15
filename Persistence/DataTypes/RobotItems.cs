using System.Diagnostics.CodeAnalysis;

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
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if(obj is Position position) {
                return position.X == X && position.Y == Y;
            }
            return base.Equals(obj);
        }
        // https://stackoverflow.com/questions/371328/why-is-it-important-to-override-gethashcode-when-equals-method-is-overridden
        public override int GetHashCode()
        {
            unchecked // overflow működésben nem okoz hibát, de exception dobna
            {
                int hash = 13;
                hash = hash * 3 + X.GetHashCode();
                hash = hash * 7 + Y.GetHashCode();
                return hash;
            }
        }

    }

    public struct RobotMove
    {
        public Position Position;
        public char Operation;
    }
}
