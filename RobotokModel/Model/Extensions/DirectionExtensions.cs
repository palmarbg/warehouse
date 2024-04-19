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
        public static List<RobotOperation> RotateTo(this Direction start, Direction end)
        {
            switch (start)
            {
                case Direction.Left:
                    switch (end)
                    {
                        case Direction.Left:
                            return new List<RobotOperation>();
                        case Direction.Up:
                            return new List<RobotOperation> { RobotOperation.Clockwise };
                        case Direction.Right:
                            return new List<RobotOperation> { RobotOperation.CounterClockwise, RobotOperation.CounterClockwise };
                        case Direction.Down:
                            return new List<RobotOperation> { RobotOperation.CounterClockwise };
                    }
                    break;
                case Direction.Up:
                    switch (end)
                    {
                        case Direction.Left:
                            return new List<RobotOperation> { RobotOperation.CounterClockwise };
                        case Direction.Up:
                            return new List<RobotOperation>();
                        case Direction.Right:
                            return new List<RobotOperation> { RobotOperation.Clockwise };
                        case Direction.Down:
                            return new List<RobotOperation> { RobotOperation.CounterClockwise, RobotOperation.CounterClockwise };
                    }
                    break;
                case Direction.Right:
                    switch (end)
                    {
                        case Direction.Left:
                            return new List<RobotOperation> { RobotOperation.CounterClockwise, RobotOperation.CounterClockwise };
                        case Direction.Up:
                            return new List<RobotOperation> { RobotOperation.CounterClockwise };
                        case Direction.Right:
                            return new List<RobotOperation>();
                        case Direction.Down:
                            return new List<RobotOperation> { RobotOperation.Clockwise };
                    }
                    break;
                case Direction.Down:
                    switch (end)
                    {
                        case Direction.Left:
                            return new List<RobotOperation> { RobotOperation.Clockwise };
                        case Direction.Up:
                            return new List<RobotOperation> { RobotOperation.CounterClockwise, RobotOperation.CounterClockwise };
                        case Direction.Right:
                            return new List<RobotOperation> { RobotOperation.CounterClockwise };
                        case Direction.Down:
                            return new List<RobotOperation>();
                    }
                    break;
            }

            return new List<RobotOperation>(); 
        }

        public static string ToChar(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => "N",
                Direction.Down => "S",
                Direction.Left => "W",
                Direction.Right =>"E",
                _ => throw new Exception()
            };
        }

        public static Direction ToDirection(this string direction)
        {
            return direction switch
            {
                "W" => Direction.Left,
                "N" => Direction.Up,
                "E" => Direction.Right,
                "S" => Direction.Down,
                _ => throw new Exception(),
            };
        }

    }
}
