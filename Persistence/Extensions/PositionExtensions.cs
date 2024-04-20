using Persistence.DataTypes;

namespace Persistence.Extensions
{
    public static class PositionExtensions
    {
        public static Position PositionInDirection(this Position position, Direction rotation)
        {
            var newPosition = new Position();
            switch (rotation)
            {
                case Direction.Left:
                    newPosition.X = position.X - 1;
                    newPosition.Y = position.Y;
                    break;
                case Direction.Up:
                    newPosition.X = position.X;
                    newPosition.Y = position.Y - 1;
                    break;
                case Direction.Right:
                    newPosition.X = position.X + 1;
                    newPosition.Y = position.Y;
                    break;
                case Direction.Down:
                    newPosition.X = position.X;
                    newPosition.Y = position.Y + 1;
                    break;
            }
            return newPosition;
        }
        public static bool EqualsPosition(this Position position1, Position position2)
        {
            return position1.X == position2.X && position1.Y == position2.Y;
        }
        // Gives null if positions are not on the same y or x coordinate
        public static Direction? DirectionInPosition(this Position start, Position end)
        {
            if (start.X == end.X)
            {
                if (start.Y == end.Y)
                    return null;
                if (start.Y > end.Y)
                    return Direction.Up;
                if (start.Y < end.Y)
                    return Direction.Down;
            }
            else if (start.Y == end.Y)
            {
                if (start.X > end.X)
                    return Direction.Left;
                if (start.X < end.X)
                    return Direction.Right;
            }
            return null;
        }
    }

}
