using RobotokModel.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Controllers
{
    internal class SimpleController : IController
    {
        public event EventHandler<IControllerEventArgs>? FinishedTask;
        private SimulationData? SimulationData;

        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan)
        {
            this.SimulationData = simulationData;
        }

        // Does not care about Block, other robots or deadlocks
        public async Task ClaculateOperations(TimeSpan timeSpan)
        {
            await Task.Run(() =>
            {
                if (SimulationData == null)
                {
                    throw new InvalidOperationException();
                }
                var result = SimulationData.Robots.Select(robot =>
                {
                    if (robot.CurrentGoal is null) return RobotOperation.Wait;
                    var robotPosition = robot.Position;
                    var goalPosition = robot.CurrentGoal.Position;
                    if (robotPosition.X == goalPosition.X)
                    {
                        int distance = robot.Position.Y - robot.CurrentGoal.Position.Y;
                        if (distance < 0)
                        {
                            switch (robot.Rotation)
                            {
                                case Direction.Left:
                                    return RobotOperation.CounterClockwise;
                                case Direction.Up:
                                    return RobotOperation.CounterClockwise;
                                case Direction.Right:
                                    return RobotOperation.Clockwise;
                                case Direction.Down:
                                    return RobotOperation.Forward;
                            }
                        }
                        if (distance > 0)
                        {
                            switch (robot.Rotation)
                            {
                                case Direction.Left:
                                    return RobotOperation.Clockwise;
                                case Direction.Up:
                                    return RobotOperation.Forward;
                                case Direction.Right:
                                    return RobotOperation.Clockwise;
                                case Direction.Down:
                                    return RobotOperation.Clockwise;
                            }

                        }
                        else return RobotOperation.Wait;
                    }
                    else
                    {
                        int distance = robot.Position.X - robot.CurrentGoal.Position.X;
                        if (distance < 0)
                        {
                            switch (robot.Rotation)
                            {
                                case Direction.Left:
                                    return RobotOperation.Clockwise;
                                case Direction.Up:
                                    return RobotOperation.Clockwise;
                                case Direction.Right:
                                    return RobotOperation.Forward;
                                case Direction.Down:
                                    return RobotOperation.CounterClockwise;
                            }
                        }
                        if (distance > 0)
                        {
                            switch (robot.Rotation)
                            {
                                case Direction.Left:
                                    return RobotOperation.Forward;
                                case Direction.Up:
                                    return RobotOperation.CounterClockwise;
                                case Direction.Right:
                                    return RobotOperation.CounterClockwise;
                                case Direction.Down:
                                    return RobotOperation.Clockwise;
                            }
                        }

                    }
                    return RobotOperation.Wait;
                });
                OnTaskFinished(result.ToArray());
            });
        }

        private void OnTaskFinished(RobotOperation[] result)
        {
            FinishedTask?.Invoke(this, new(result));
        }
    }
}
