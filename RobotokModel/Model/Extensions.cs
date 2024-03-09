using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model
{

    public static class DirectionExtenstions
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
    }

}
