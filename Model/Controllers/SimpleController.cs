using Model.DataTypes;
using Model.Interfaces;
using Persistence.DataTypes;
using System.Diagnostics.CodeAnalysis;

namespace Model.Controllers
{
    [ExcludeFromCodeCoverage]
    internal class SimpleController : IController
    {
        public event EventHandler<IControllerEventArgs>? FinishedTask;
        public event EventHandler? InitializationFinished;

        private ITaskDistributor _taskDistributor = null!;
        private SimulationData? SimulationData;

        public string Name => "simple";
        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan, ITaskDistributor distributor, CancellationToken? token = null)
        {
            _taskDistributor = distributor;
            SimulationData = simulationData;
            InitializationFinished?.Invoke(this, new());
        }

        // Does not care about Block, other robots or deadlocks
        public void CalculateOperations(TimeSpan timeSpan, CancellationToken? token = null)
        {

            if (SimulationData == null)
            {
                throw new InvalidOperationException();
            }
            var result = SimulationData.Robots.Select(robot =>
            {
                if (robot.CurrentGoal is null)
                {
                    if (_taskDistributor.AllTasksAssigned)
                    {
                        robot.NextOperation = RobotOperation.Wait;
                        return robot.NextOperation;
                    }
                    _taskDistributor.AssignNewTask(robot);

                    //Goal.OnGoalsChanged();
                    if (robot.CurrentGoal is null)
                    {
                        robot.NextOperation = RobotOperation.Wait;
                        return robot.NextOperation;
                    }
                }
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
                                robot.NextOperation = RobotOperation.CounterClockwise;
                                break;
                            case Direction.Up:
                                robot.NextOperation = RobotOperation.CounterClockwise;
                                break;
                            case Direction.Right:
                                robot.NextOperation = RobotOperation.Clockwise;
                                break;
                            case Direction.Down:
                                robot.NextOperation = RobotOperation.Forward;
                                break;
                        }
                    }
                    else if (distance > 0)
                    {
                        switch (robot.Rotation)
                        {
                            case Direction.Left:
                                robot.NextOperation = RobotOperation.Clockwise;
                                break;
                            case Direction.Up:
                                robot.NextOperation = RobotOperation.Forward;
                                break;
                            case Direction.Right:
                                robot.NextOperation = RobotOperation.Clockwise;
                                break;
                            case Direction.Down:
                                robot.NextOperation = RobotOperation.Clockwise;
                                break;
                        }
                    }
                    else robot.NextOperation = RobotOperation.Wait;
                }
                else
                {
                    int distance = robot.Position.X - robot.CurrentGoal.Position.X;
                    if (distance < 0)
                    {
                        switch (robot.Rotation)
                        {
                            case Direction.Left:
                                robot.NextOperation = RobotOperation.Clockwise;
                                break;
                            case Direction.Up:
                                robot.NextOperation = RobotOperation.Clockwise;
                                break;
                            case Direction.Right:
                                robot.NextOperation = RobotOperation.Forward;
                                break;
                            case Direction.Down:
                                robot.NextOperation = RobotOperation.CounterClockwise;
                                break;
                        }
                    }
                    else if (distance > 0)
                    {
                        switch (robot.Rotation)
                        {
                            case Direction.Left:
                                robot.NextOperation = RobotOperation.Forward;
                                break;
                            case Direction.Up:
                                robot.NextOperation = RobotOperation.CounterClockwise;
                                break;
                            case Direction.Right:
                                robot.NextOperation = RobotOperation.CounterClockwise;
                                break;
                            case Direction.Down:
                                robot.NextOperation = RobotOperation.Clockwise;
                                break;
                        }
                    }
                    else robot.NextOperation = RobotOperation.Wait;
                }
                return robot.NextOperation;
            });
            OnTaskFinished(result.ToArray());

        }

        private void OnTaskFinished(RobotOperation[] result)
        {
            FinishedTask?.Invoke(this, new(result));
        }

        public IController NewInstance()
        {
            return new SimpleController();
        }
    }
}
