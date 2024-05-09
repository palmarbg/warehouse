using Model.Interfaces;
using Persistence.DataTypes;
using Persistence.Extensions;
using Persistence.Interfaces;
using System.Diagnostics;

namespace Model.Executors
{
    public class DefaultExecutor : ExecutorBase, IExecutor
    {

        private EventHandler<Goal> _robotTaskAssignedDelegate;

        public DefaultExecutor(SimulationData simulationData, ILogger logger) : base(simulationData, logger)
        {
            _robotTaskAssignedDelegate = (robot, goal) => OnTaskAssigned(goal.Id, ((Robot)robot!).Id);

            Robot.TaskAssigned += _robotTaskAssignedDelegate;
        }

        /// <summary>
        /// Doesn't handle deadlock
        /// </summary>
        /// <param name="robotOperations"></param>
        public RobotOperation[] ExecuteOperations(RobotOperation[] robotOperations, float timeSpan)
        {
            throw new NotImplementedException();
            _errors = new List<OperationError>();
            // Reset MovedThisTurn
            for (int i = 0; i < _simulationData.Robots.Count; i++)
            {
                _simulationData.Robots[i].MovedThisTurn = false;
                _simulationData.Robots[i].InspectedThisTurn = false;
                _simulationData.Robots[i].BlockedThisTurn = false;

            }

            RobotOperation[] executedOperations = new RobotOperation[robotOperations.Length];

            //Execute operations
            for (int i = 0; i < _simulationData.Robots.Count; i++)
            {
                Robot robot = _simulationData.Robots[i];
                MoveRobot(robot, robot);
                if (robot.BlockedThisTurn)
                {
                    executedOperations[i] = RobotOperation.Wait;
                }
                else
                {
                    executedOperations[i] = robotOperations[i];
                }
            }

            OnStepFinished(robotOperations, executedOperations, _errors.ToArray(), timeSpan);
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
                    if (newPos.Y >= _simulationData.Map.GetLength(1) || newPos.X >= _simulationData.Map.GetLength(0) || newPos.Y < 0 || newPos.X < 0)
                    {
                        robot.MovedThisTurn = true;
                        robot.BlockedThisTurn = true;
                        OnWallHit(robot.Id);
                        return false;
                    }
                    // robot is blocked by Block
                    if (_simulationData.Map.GetAtPosition(newPos) is Block)
                    {
                        robot.MovedThisTurn = true;
                        robot.BlockedThisTurn = true;
                        OnWallHit(robot.Id);
                        return false;
                    }
                    //newPos is blocked by another robot
                    else if (_simulationData.Map[newPos.X, newPos.Y] is Robot blockingRobot)
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
                                //robot.BlockedThisTurn = true;
                                //robot.MovedThisTurn = true;
                                //OnRobotCrash(robot.Id, blockingRobot.Id);

                                robot.Position = blockingRobot.Position;
                                robot.MovedThisTurn= true;
                                if (robot.CurrentGoal!.Position.EqualsPosition(blockingRobot.Position))
                                {
                                    robot.CurrentGoal.IsAssigned = false;
                                    OnTaskFinished(robot.CurrentGoal.Id, robot.Id);
                                    robot.CurrentGoal = null;
                                }

                                return true;
                            }
                            else
                            if (!blockingRobot.InspectedThisTurn && MoveRobot(blockingRobot, startingRobot))
                            {
                                MoveRobotToNewPosition(robot, newPos, operation);

                                if (newPos.X == robot.CurrentGoal?.Position.X && newPos.Y == robot.CurrentGoal?.Position.Y)
                                {
                                    //MoveRobotToNewPosition(robot, newPos, operation);
                                    //simulationData.Goals.Remove(robot.CurrentGoal);
                                    robot.CurrentGoal.IsAssigned = false;
                                    OnTaskFinished(robot.CurrentGoal.Id, robot.Id);
                                    robot.CurrentGoal = null;
                                    robot.MovedThisTurn = true;
                                    return true;

                                }

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
                        OnTaskFinished(robot.CurrentGoal.Id, robot.Id);
                        robot.CurrentGoal = null;
                        robot.MovedThisTurn = true;
                        return true;

                    }
                    // newPos is empty or another robots goal
                    else if (_simulationData.Map[newPos.X, newPos.Y].IsPassable)
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
                    robot.MovedThisTurn = true;
                    if(robot.CurrentGoal is not null && robot.Position.EqualsPosition(robot.CurrentGoal.Position))
                    {
                        robot.CurrentGoal.IsAssigned = false;
                        OnTaskFinished(robot.CurrentGoal.Id, robot.Id);
                        robot.CurrentGoal = null;
                    }
                    //robot.BlockedThisTurn = true;
                    return false;
                    //break;
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
            var map = _simulationData.Map;
            var temp = map.GetAtPosition(newPosition);
            map.SetAtPosition(newPosition, robot);
            map.SetAtPosition(robot.Position, temp);
            robot.Position = newPosition;
        }

        public void Timeout()
        {
            OnTimeout();
        }

        public void SaveSimulationLog(string filepath)
        {
            _logger.SaveLog(filepath);
        }

        public IExecutor NewInstance(SimulationData simulationData)
        {
            return new DefaultExecutor(simulationData, _logger.NewInstance(simulationData));
        }

        public void Dispose()
        {
            Robot.TaskAssigned -= _robotTaskAssignedDelegate;
        }

    }
}
