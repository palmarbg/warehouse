using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model
{

    public static class Extenstions
    {
        #region Position extensions

        public static Position PoistionInDirection(this Position position, Direction rotation)
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

        #endregion

        #region Direction extensions

        public static void RotateClockWise(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    break;
                case Direction.Up:
                    break;
                case Direction.Right:
                    break;
                case Direction.Down:
                    break;
            }
        }
        public static void RotateCounterClockWise(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    break;
                case Direction.Up:
                    break;
                case Direction.Right:
                    break;
                case Direction.Down:
                    break;
            }
        }

        #endregion

        #region RobotOperation extensions

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
                    c = 'C';
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

        public static RobotOperation ToRobotOperation(this char operationChar)
        {
            //TODO: Prototype 2: Custom Exceptions
            return operationChar switch
            {
                'F' => RobotOperation.Forward,
                'W' => RobotOperation.Wait,
                'C' => RobotOperation.CounterClockwise,
                'R' => RobotOperation.Clockwise,
                'B' => RobotOperation.Backward,
                _ => throw new Exception(),
            };
        }

        public static RobotOperation Reverse(this RobotOperation operation)
        {
            return operation switch
            {
                RobotOperation.Forward => RobotOperation.Backward,
                RobotOperation.Clockwise => RobotOperation.CounterClockwise,
                RobotOperation.CounterClockwise => RobotOperation.Clockwise,
                RobotOperation.Backward => RobotOperation.Forward,
                RobotOperation.Wait => RobotOperation.Wait,
                _ => RobotOperation.Wait,
            };
        }

        #endregion

        #region Map extensions

        /// <summary>
        /// This wont work if place is taken(?)
        /// </summary>
        public static void MoveRobotToNewPosition(this ITile[,] map, Robot robot, Position newPosition, RobotOperation operaition)
        {
            map.SetAtPosition(newPosition, robot);
            map.SetAtPosition(robot.Position, EmptyTile.Instance);
            robot.Position = newPosition;

            var rmEvent = new RobotMove
            {
                Position = newPosition,
                Operation = operaition.ToChar()
            };
        }

        #endregion

        #region Robot extensions

        /// <summary>
        /// Moves robot to new Position and resolves robots blocking each other
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="simulation"></param>
        /// <returns>
        /// <c>True</c> if <c>robot</c> moved away from original position
        /// <para/>
        /// <c>False</c> if <c>robot</c> was blocked or if it's rotating
        /// </returns>
        public static bool MoveRobot(this Robot robot, Simulation simulation)
        {
            var operation = robot.NextOperation;
            switch (operation)
            {
                case RobotOperation.Forward:
                    var newPos = robot.Position.PoistionInDirection(robot.Rotation);
                    // newPos is empty or another robots goal
                    if (newPos.X == robot.CurrentGoal.Position.X && newPos.Y == robot.CurrentGoal.Position.Y)
                    {
                        simulation.SimulationData.Map.MoveRobotToNewPosition(robot, newPos, operation);
                        robot.MovedThisTurn = true;
                        return true;
                    }
                    // newPos is robots goal
                    else if (simulation.SimulationData.Map[newPos.X, newPos.Y].IsPassable)
                    {
                        simulation.SimulationData.Map.MoveRobotToNewPosition(robot, newPos, operation);
                        simulation.OnGoalsChanged();
                        simulation.SimulationData.Goals.Remove(robot.CurrentGoal);
                        simulation.Distributor.AssignNewTask(robot);
                        robot.MovedThisTurn = true;
                        return true;
                    }
                    //newPos is blocked by another robot
                    else if (simulation.SimulationData.Map[newPos.X, newPos.Y] is Robot blockingRobot)
                    {
                        if (blockingRobot.MovedThisTurn)
                        {
                            robot.MovedThisTurn = true;
                            return false;
                        }
                        else
                        {
                            // TODO: Check if robot was blocking original robots goal
                            if (blockingRobot.MoveRobot(simulation))
                            {
                                simulation.SimulationData.Map.MoveRobotToNewPosition(robot, newPos, operation);
                                return true;
                            }
                            else
                            {
                                robot.MovedThisTurn = true;
                                return false;
                            }
                        }

                    }
                    // robot is blocked by Block
                    else
                    {
                        robot.MovedThisTurn = true;
                        return false;
                    }

                //break;
                case RobotOperation.Clockwise:
                    robot.Rotation.RotateClockWise();
                    break;
                case RobotOperation.CounterClockwise:
                    robot.Rotation.RotateCounterClockWise();
                    break;
                case RobotOperation.Backward:
                    // TODO: Prototype 2
                    break;
                case RobotOperation.Wait:
                    // TODO: Prototype 2 : Logging
                    break;
            }
            return false;

        }

        #endregion

        #region Matrix extension

        public static T GetAtPosition<T>(this T[,] matrix, Position position)
        {
            return matrix[position.X, position.Y];
        }
        public static void SetAtPosition<T>(this T[,] matrix, Position position, T newItem)
        {
            matrix[position.X, position.Y] = newItem;
        }

        #endregion
    }

}
