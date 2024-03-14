using RobotokModel.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RobotokModel.Model
{
    public class Simulation
    {
        private SimulationData SimulationData { get; set; }
        private List<List<RobotOperation>> ExecutedOperations { get; set; } = new List<List<RobotOperation>>();
        private Log CurrentLog { get; set; }
        private ITaskDistributor Distributor { get; set; }
        private IController Controller { get; set; }
        private int RemainingSteps { get; set; }
        public EventHandler<RobotMove> RobotMovedEvent { get; set; }
        public EventHandler<int> GoalFinished { get; set; }
        public EventHandler<Log> SimulationFinished { get; set; }

        private System.Timers.Timer Timer;
        private bool SimulationRunning;


        public Simulation(Config config, string Strategy, int StepLimit)
        {

            Timer = new System.Timers.Timer();
            Timer.Interval = 1000;
            Timer.Enabled = true;
            Timer.Elapsed += SimulationStep;
            SimulationRunning = false;


            throw new NotImplementedException();

        }
        public void StepForward()
        {
            throw new NotImplementedException();
        }

        public void StepBackward()
        {
            throw new NotImplementedException();
        }
        public void StartSimulation()
        {
            SimulationRunning = true;
            Timer.Start();

            throw new NotImplementedException();
        }



        public void StopSimulation()
        {
            if (SimulationRunning)
            {
                SimulationRunning = false;
                Timer.Stop();
                SimulationFinished.Invoke(this, this.GetLog());
            }
            else throw new NotImplementedException();

            throw new NotImplementedException();
        }
        public void PauseSimulation()
        {
            throw new NotImplementedException();
        }
        public void SetSimulationSpeed()
        {
            throw new NotImplementedException();
        }
        public void JumpToStep(int step)
        {
            throw new NotImplementedException();
        }
        public Log GetLog()
        {
            throw new NotImplementedException();
        }
        private int GoalsRemaining()
        {
            throw new NotImplementedException();
        }
        // TODO: Prototype 2 : Log planned and executed moves
        private void SimulationStep(object? sender, ElapsedEventArgs args)
        {
            var operations = Controller.NextStep();
            for (int i = 0; i < Robot.Robots.Count; i++)
            {
                var robot = Robot.Robots[i];
                if (!robot.MovedThisTurn) MoveRobot(robot);
            }
            Robot.EndTurn();

        }
        /// <summary>
        /// Moves robot to new Position and resolves robots blocking each other
        /// </summary>
        /// <param name="robot"></param>
        /// <returns>
        /// true if robot moved away from original position
        /// false if robot was blocked or if it is rotating
        /// </returns>
        private bool MoveRobot(Robot robot)
        {
            var operation = robot.NextOperation;
            switch (operation)
            {
                case RobotOperation.Forward:
                    var newPos = PoistionInDirection(robot.Rotation, robot.Position);
                    // newPos is empty or another robots goal
                    if (newPos.X == robot.CurrentGoal.Position.X && newPos.Y == robot.CurrentGoal.Position.Y)
                    {
                        MoveRobotToNewPosition(robot, newPos, operation);
                        robot.MovedThisTurn = true;
                        return true;
                    }
                    // newPos is robots goal
                    else if (SimulationData.Map[newPos.X, newPos.Y].IsPassable)
                    {
                        MoveRobotToNewPosition(robot, newPos, operation);
                        GoalFinished.Invoke(this, robot.CurrentGoal.Id);
                        SimulationData.Goals.Remove(robot.CurrentGoal);
                        Distributor.AssignNewTask(robot);
                        robot.MovedThisTurn = true;
                        return true;
                    }
                    //newPos is blocked by another robot
                    else if (SimulationData.Map[newPos.X, newPos.Y] is Robot blockingRobot)
                    {
                        if (blockingRobot.MovedThisTurn)
                        {
                            robot.MovedThisTurn = true;
                            return false;
                        }
                        else
                        {
                            // TODO: Check if robot was blocking original robots goal
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

        private void MoveRobotToNewPosition(Robot robot, Position newPosition, RobotOperation operaition)
        {
            SimulationData.Map[newPosition.X, newPosition.Y] = robot;
            SimulationData.Map[robot.Position.X, robot.Position.Y] = new EmptyTile();
            robot.Position = newPosition;

            var rmEvent = new RobotMove();
            rmEvent.Position = newPosition;
            rmEvent.Operation = operaition.ToChar();
            RobotMovedEvent.Invoke(this, rmEvent);
        }
        private Position PoistionInDirection(Direction rotation, Position position)
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
                    newPosition.Y = position.Y + 1;
                    break;
                case Direction.Right:
                    newPosition.X = position.X;
                    newPosition.Y = position.Y + 1;
                    break;
                case Direction.Down:
                    newPosition.X = position.X;
                    newPosition.Y = position.Y - 1;
                    break;
            }
            return newPosition;
        }
    }
}
