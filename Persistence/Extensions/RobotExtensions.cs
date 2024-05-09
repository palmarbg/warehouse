using Persistence.DataTypes;

namespace Persistence.Extensions
{
    public static class RobotExtensions
    {
        public static void ExecuteMove(this Robot robot)
        {
            switch (robot.NextOperation)
            {
                case RobotOperation.Forward:
                    robot.Position = robot.Position.PositionInDirection(robot.Rotation);
                    break;
                case RobotOperation.Backward:
                    robot.Position = robot.Position.PositionInDirection(robot.Rotation.Opposite());
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

        public static void ExecuteMove(this Robot robot, RobotOperation robotOperation)
        {
            switch (robotOperation)
            {
                case RobotOperation.Forward:
                    robot.Position = robot.Position.PositionInDirection(robot.Rotation);
                    break;
                case RobotOperation.Backward:
                    robot.Position = robot.Position.PositionInDirection(robot.Rotation.Opposite());
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
