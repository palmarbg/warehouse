using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Extensions
{
    public static class RobotExtensions
    {
        public static void ExecuteMove(this Robot robot)
        {
            switch (robot.NextOperation)
            {
                case RobotOperation.Forward:
                    robot.Position = robot.Position.PoistionInDirection(robot.Rotation);
                    break;
                case RobotOperation.Backward:
                    robot.Position = robot.Position.PoistionInDirection(robot.Rotation.Opposite());
                    break;
                case RobotOperation.Clockwise:
                    robot.Rotation = robot.Rotation.RotateClockWise();
                    break;
                case RobotOperation.CounterClockwise:
                    robot.Rotation = robot.Rotation.RotateCounterClockWise();
                    break;
                case RobotOperation.Wait:
                default:
                    break;
            }
            robot.MovedThisTurn = true;
        }
    }
}
