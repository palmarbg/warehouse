using RobotokModel.Model.Extensions;
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

        //THESE WILL BE REFACTORED: ?Executor.cs
        
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
                    if (newPos.X == robot.CurrentGoal?.Position.X && newPos.Y == robot.CurrentGoal?.Position.Y)
                    {
                        simulation.simulationData.Map.MoveRobotToNewPosition(robot, newPos, operation);
                        robot.MovedThisTurn = true;
                        return true;
                    }
                    // newPos is robots goal
                    else if (simulation.simulationData.Map[newPos.X, newPos.Y].IsPassable)
                    {
                        simulation.simulationData.Map.MoveRobotToNewPosition(robot, newPos, operation);
                        Goal.OnGoalsChanged();
                        if(robot.CurrentGoal != null)
                            simulation.simulationData.Goals.Remove(robot.CurrentGoal.Value);
                            
                        simulation.Distributor.AssignNewTask(robot);
                        robot.MovedThisTurn = true;
                        return true;
                    }
                    //newPos is blocked by another robot
                    else if (simulation.simulationData.Map[newPos.X, newPos.Y] is Robot blockingRobot)
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
                                simulation.simulationData.Map.MoveRobotToNewPosition(robot, newPos, operation);
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



    }

}
