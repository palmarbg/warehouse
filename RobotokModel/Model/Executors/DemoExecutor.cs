using RobotokModel.Model.Extensions;
using RobotokModel.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Executors
{
    public class DemoExecutor : IExecutor
    {
        private readonly SimulationData simulationData;
        public DemoExecutor(SimulationData simulationData)
        {
            this.simulationData = simulationData;
        }

        /// <summary>
        /// Doesn't handle deadlock
        /// </summary>
        /// <param name="robotOperations"></param>
        public RobotOperation[] ExecuteOperations(RobotOperation[] robotOperations)
        {
            //for (int i = 0; i < simulationData.Robots.Count; i++)
            //{
            //    Robot robot = simulationData.Robots[i];
            //    robot.ExecuteMove();
            //}

            // Reset MovedThisTurn
            for (int i = 0; i < simulationData.Robots.Count; i++)
            {
                simulationData.Robots[i].MovedThisTurn = false;
            }
            //Execute operations
            for (int i = 0; i < simulationData.Robots.Count; i++)
            {
                MoveRobot(simulationData.Robots[i]);
            }
            
            return robotOperations;
        }

        /// <summary>
        /// Moves the given robot
        /// Moves all robots this robots new move is dependent on
        /// </summary>
        /// <param name="robot"></param>
        /// <returns></returns>
        //TODO aktuális mozgások logolása
        //TODO Robotok körbe akarnak menni, akkor beakadnak
        private bool MoveRobot(Robot robot)
        {
            if (robot.MovedThisTurn)
            {
                return false;
            }
            var operation = robot.NextOperation;
            switch (operation)
            {
                case RobotOperation.Forward:
                    var newPos = robot.Position.PositionInDirection(robot.Rotation);
                    if (newPos.X >= simulationData.Map.GetLength(0) || newPos.Y >= simulationData.Map.GetLength(1))
                    {
                        robot.MovedThisTurn = true;
                        return false;
                    }
                    // newPos is robots goal
                    if (newPos.X == robot.CurrentGoal?.Position.X && newPos.Y == robot.CurrentGoal?.Position.Y)
                    {
                        MoveRobotToNewPosition(robot, newPos, operation);
                        Goal.OnGoalsChanged();
                        if (robot.CurrentGoal != null)
                            simulationData.Goals.Remove(robot.CurrentGoal.Value);

                        //Distributor.AssignNewTask(robot);
                        robot.MovedThisTurn = true;
                        return true;

                    }
                    // newPos is empty or another robots goal
                    else if (simulationData.Map[newPos.X, newPos.Y].IsPassable)
                    {
                        MoveRobotToNewPosition(robot, newPos, operation);
                        robot.MovedThisTurn = true;
                        return true;
                    }
                    //newPos is blocked by another robot
                    else if (simulationData.Map[newPos.X, newPos.Y] is Robot blockingRobot)
                    {
                        if (blockingRobot.MovedThisTurn)
                        {
                            robot.MovedThisTurn = true;
                            return false;
                        }
                        else
                        {
                            // TODO: Check if robot was blocking original robots NewPos
                            if (MoveRobot(blockingRobot))
                            {
                                MoveRobotToNewPosition(robot, newPos, operation);
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
        /// Moves robot to new position without checking the legality of the move
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="newPosition"></param>
        /// <param name="operaition"></param>
        private void MoveRobotToNewPosition( Robot robot, Position newPosition, RobotOperation operaition)
        {
            var map = simulationData.Map;
            map.SetAtPosition(newPosition, robot);
            map.SetAtPosition(robot.Position, EmptyTile.Instance);
            robot.Position = newPosition;
        }

        public void Timeout()
        {
            throw new NotImplementedException();
        }
    }
}
