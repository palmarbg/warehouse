using RobotokModel.Model.Extensions;
using RobotokModel.Model.Interfaces;
using RobotokModel.Persistence;
using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RobotokModel.Model.Executors
{
    public class DefaultExecutor : IExecutor
    {
        private readonly SimulationData simulationData;
        private readonly ILogger logger;
        private List<OperationError> errors = null!;

        public DefaultExecutor(SimulationData simulationData, ILogger logger)
        {
            this.simulationData = simulationData;
            this.logger = logger;
        }

        /// <summary>
        /// Doesn't handle deadlock
        /// </summary>
        /// <param name="robotOperations"></param>
        public RobotOperation[] ExecuteOperations(RobotOperation[] robotOperations, float timeSpan)
        {
            errors = new List<OperationError>();
            // Reset MovedThisTurn
            for (int i = 0; i < simulationData.Robots.Count; i++)
            {
                simulationData.Robots[i].MovedThisTurn = false;
                simulationData.Robots[i].InspectedThisTurn = false;
                simulationData.Robots[i].BlockedThisTurn = false;

            }

            RobotOperation[] executedOperations = new RobotOperation[robotOperations.Length];

            //Execute operations
            for (int i = 0; i < simulationData.Robots.Count; i++)
            {
                Robot robot = simulationData.Robots[i];
                MoveRobot(robot, robot);
                if (robot.BlockedThisTurn)
                {
                    executedOperations[i] = RobotOperation.Wait;
                } else
                {
                    executedOperations[i] = robotOperations[i];
                }
            }

            OnStepFinished(robotOperations, executedOperations, errors.ToArray(), timeSpan);

            return robotOperations;
        }

        /// <summary>
        /// Moves the given robot.
        /// Moves all robots this robots new move is dependent on.
        /// </summary>
        /// <param name="robot"></param>
        /// <returns>did the robot move</returns>
        //TODO aktuális mozgások logolása
        //TODO Robotok körbe akarnak menni, akkor beakadnak
        private bool MoveRobot(Robot robot, Robot startingRobot)
        {
            robot.InspectedThisTurn = true;
            if (robot.MovedThisTurn) return false;
            var operation = robot.NextOperation;
            switch (operation)
            {
                case RobotOperation.Forward:
                    var newPos = robot.Position.PositionInDirection(robot.Rotation);
                    if (newPos.Y >= simulationData.Map.GetLength(1) || newPos.X >= simulationData.Map.GetLength(0) || newPos.Y < 0 || newPos.X < 0)
                    {
                        robot.MovedThisTurn = true;
                        robot.BlockedThisTurn = true;
                        OnWallHit(robot.Id);
                        return false;
                    }
                    // robot is blocked by Block
                    if (simulationData.Map.GetAtPosition(newPos) is Block)
                    {
                        robot.MovedThisTurn = true;
                        OnWallHit(robot.Id);
                        return false;
                    }
                    //newPos is blocked by another robot
                    else if (simulationData.Map[newPos.X, newPos.Y] is Robot blockingRobot)
                    {
                        if (blockingRobot.MovedThisTurn)
                        {
                            robot.BlockedThisTurn = true;
                            robot.MovedThisTurn = true;
                            OnRobotCrash(robot.Id, blockingRobot.Id);
                            return false;
                        }
                        else
                        {
                            // TODO: Check if robot was blocking original robots NewPos
                            if (startingRobot.Id == blockingRobot.Id)
                            {
                                robot.BlockedThisTurn = true;
                                robot.MovedThisTurn = true;
                                return false;
                            }
                            else
                            if (!blockingRobot.InspectedThisTurn && MoveRobot(blockingRobot, startingRobot))
                            {
                                MoveRobotToNewPosition(robot, newPos, operation);
                                return true;
                            }
                            else
                            {
                                robot.BlockedThisTurn = true;
                                robot.MovedThisTurn = true;
                                OnRobotCrash(robot.Id, blockingRobot.Id);
                                return false;
                            }
                        }
                    }
                    // newPos is robots goal
                    else if (newPos.X == robot.CurrentGoal?.Position.X && newPos.Y == robot.CurrentGoal?.Position.Y)
                    {
                        MoveRobotToNewPosition(robot, newPos, operation);
                        //simulationData.Goals.Remove(robot.CurrentGoal);
                        robot.CurrentGoal.IsAssigned = false;
                        OnTaskFinished(robot.CurrentGoal.Id,robot.Id);
                        robot.CurrentGoal = null;
                        Goal.OnGoalsChanged();
                        //TODO: Robotnak új goal-t kell adni
                        //robot.CurrentGoal = null;
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
                    else
                    {
                        throw new Exception();
                        //robot.BlockedThisTurn = true;
                        //robot.MovedThisTurn = true;
                        //return false;
                    }
                //break;
                case RobotOperation.Clockwise:
                    robot.Rotation = robot.Rotation.RotateClockWise();
                    robot.MovedThisTurn = true;
                    break;
                case RobotOperation.CounterClockwise:
                    robot.Rotation = robot.Rotation.RotateCounterClockWise();
                    robot.MovedThisTurn = true;
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
        private void MoveRobotToNewPosition(Robot robot, Position newPosition, RobotOperation operaition)
        {
            var map = simulationData.Map;
            map.SetAtPosition(newPosition, robot);
            map.SetAtPosition(robot.Position, EmptyTile.Instance);
            robot.Position = newPosition;
        }

        public IExecutor NewInstance(SimulationData simulationData)
        {
            return new DefaultExecutor(simulationData, logger.NewInstance(simulationData));
        }

        public void Timeout()
        {
            OnTimeout();
        }

        public void TaskAssigned(int taskId, int robotId)
        {
            logger.LogEvent(new(taskId, simulationData.Step, TaskEventType.assigned), robotId);
        }

        public void SaveSimulation(string filepath)
        {
            logger.SaveLog(filepath);
        }

        #region Private methods

        private void OnTaskFinished(int taskId, int robotId)
        {
            logger.LogEvent(new(taskId, simulationData.Step, TaskEventType.finished), robotId);
        }

        private void OnWallHit(int robotId)
        {
            errors.Add(new(robotId, -1, simulationData.Step, OperationErrorType.wallhit));
        }

        private void OnTimeout()
        {
            logger.LogTimeout();
            errors.Add(new(-1, -1, simulationData.Step, OperationErrorType.timeout));
        }

        private void OnRobotCrash(int robotId1, int robotId2)
        {
            errors.Add(new(robotId1, robotId2, simulationData.Step, OperationErrorType.collision));
        }

        private void OnStepFinished(
            RobotOperation[] controllerOperations,
            RobotOperation[] robotOperations,
            OperationError[] errors,
            float timeElapsed
        )
        {
            logger.LogStep(
                controllerOperations,
                robotOperations,
                errors,
                timeElapsed
            );
        }

        #endregion
    }
}
