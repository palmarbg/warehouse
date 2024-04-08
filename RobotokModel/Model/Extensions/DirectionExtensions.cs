using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Extensions
{
    public static class DirectionExtensions
    {
        public static Direction RotateClockWise(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => throw new Exception()
            };
        }
        public static Direction RotateCounterClockWise(this Direction direction)
        {
            return direction.RotateClockWise().Opposite();
        }

        public static Direction Opposite(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => throw new Exception()
            };
        }
        public static (RobotOperation operation, int count) RotateTo(this Direction start, Direction end)
        {
            switch (start)
            {
                case Direction.Left:
                    switch (end)
                    {
                        case Direction.Left:
                            return (RobotOperation.Wait, 0);
                        case Direction.Up:
                            return (RobotOperation.CounterClockwise, 1);
                        case Direction.Right:
                            return (RobotOperation.CounterClockwise, 2);
                        case Direction.Down:
                            return (RobotOperation.Clockwise, 1);
                    }
                    break;
                case Direction.Up:
                    switch (end)
                    {
                        case Direction.Left:
                            return (RobotOperation.Clockwise, 1);
                        case Direction.Up:
                            return (RobotOperation.Wait, 0);
                        case Direction.Right:
                            return (RobotOperation.CounterClockwise, 1);
                        case Direction.Down:
                            return (RobotOperation.CounterClockwise, 2);
                    }
                    break;
                case Direction.Right:
                    switch (end)
                    {
                        case Direction.Left:
                            return (RobotOperation.CounterClockwise, 2);
                        case Direction.Up:
                            return (RobotOperation.Clockwise, 1);
                        case Direction.Right:
                            return (RobotOperation.Wait, 0);
                        case Direction.Down:
                            return (RobotOperation.CounterClockwise, 1);
                    }
                    break;
                case Direction.Down:
                    switch (end)
                    {
                        case Direction.Left:
                            return (RobotOperation.CounterClockwise, 1);
                        case Direction.Up:
                            return (RobotOperation.CounterClockwise, 2);
                        case Direction.Right:
                            return (RobotOperation.Clockwise, 1);
                        case Direction.Down:
                            return (RobotOperation.Wait, 0);
                    }
                    break;
            }
            return (RobotOperation.Wait, 0);

        }
    }
}
