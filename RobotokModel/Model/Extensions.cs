using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model
{

    public static class Extenstions
    {
        public static void RotateClockWise(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    direction = Direction.Up;
                    break;
                case Direction.Up:
                    direction = Direction.Right;
                    break;
                case Direction.Right:
                    direction = Direction.Down;
                    break;
                case Direction.Down:
                    direction = Direction.Left;
                    break;
            }
        }
        public static void RotateCounterClockWise(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    direction = Direction.Down;
                    break;
                case Direction.Up:
                    direction = Direction.Left;
                    break;
                case Direction.Right:
                    direction = Direction.Up;
                    break;
                case Direction.Down:
                    direction = Direction.Right;
                    break;
            }
        }
        public static char ToChar(this RobotOperation operation)
        {
            char c = 'W';
            switch (operation)
            {
                case RobotOperation.Forward:
                    c = 'F';
                    break;
                case RobotOperation.Clockwise:
                    c = 'R';
                    break;
                case RobotOperation.CounterClockwise:
                    c = 'L';
                    break;
                case RobotOperation.Backward:
                    c = 'B';
                    break;
                case RobotOperation.Wait:
                    c = 'W';
                    break;
            }
            return c;
        }
    }

}
