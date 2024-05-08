using Model.DataTypes;
using Model.Interfaces;
using Persistence.DataTypes;
using Persistence.Extensions;
using Persistence.Interfaces;
using System.Diagnostics;

namespace Model.Utils.ReplayMediatorUtils
{
    public class ReplayController : IReplayController
    {
        private readonly ILoadLogDataAccess loadLogDataAccess = null!;
        private SimulationData simulationData = null!;
        private List<TaskEvent[]> taskEvents;
        private int[] taskEventIterator = null!;
        public string Name => "ReplayController";

        public event EventHandler<IControllerEventArgs>? FinishedTask;
        public event EventHandler? InitializationFinished;

        public ReplayController(ILoadLogDataAccess loadLogDataAccess)
        {
            this.loadLogDataAccess = loadLogDataAccess;
            taskEvents = loadLogDataAccess.GetTaskEvents();
        }

        public void CalculateOperations(TimeSpan timeSpan, CancellationToken? token = null)
        {
            if (simulationData.Step >= loadLogDataAccess.GetStepCount())
                throw new SimulationStepOutOfRangeException();

            RobotOperation[] robotOperations;
            robotOperations = loadLogDataAccess.GetRobotOperations(simulationData.Step);

            for (int i = 0; i < taskEventIterator.Length; i++)
            {
                var robot = simulationData.Robots[i];
                taskEventIterator[i] = Math.Max(taskEventIterator[i], 0);
                var iter = taskEventIterator[i];

                var currentGoal = robot.CurrentGoal;
                while (iter < taskEvents[i].Length && taskEvents[i][iter].step <= simulationData.Step)
                {
                    TaskEvent taskEvent = taskEvents[i][iter];

                    Goal goal = simulationData.Goals[taskEvent.taskId];
                    if (goal.Id != taskEvent.taskId)
                        throw new Exception();

                    if (taskEvent.eventType == TaskEventType.assigned)
                        currentGoal = goal;
                    else
                        currentGoal = null;

                    iter = ++taskEventIterator[i];
                }
                if (currentGoal != robot.CurrentGoal)
                    robot.CurrentGoal = currentGoal;
            }

            OnTaskFinished(robotOperations);
        }

        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan, ITaskDistributor distributor, CancellationToken? token = null)
        {
            this.simulationData = simulationData;
            taskEventIterator = Enumerable.Repeat(0, simulationData.Robots.Count).ToArray();
            InitializationFinished?.Invoke(this, new());
        }

        public IController NewInstance()
        {
            return new ReplayController(loadLogDataAccess);
        }

        private void OnTaskFinished(RobotOperation[] result)
        {
            FinishedTask?.Invoke(this, new(result));
        }

        #region Replay

        public void CalculateBackward()
        {
            RobotOperation[] robotOperations = loadLogDataAccess.GetRobotOperations(simulationData.Step);
            for (int i = 0; i < taskEventIterator.Length; i++)
            {
                var robot = simulationData.Robots[i];
                robotOperations[i] = robotOperations[i].Reverse();

                var iter = taskEventIterator[i] - 1;

                while (iter >= 0 && taskEvents[i][iter].step == simulationData.Step)
                {
                    TaskEvent taskEvent = taskEvents[i][iter];
                    Goal goal = simulationData.Goals[taskEvent.taskId];
                    if (taskEvent.eventType == TaskEventType.assigned)
                        robot.CurrentGoal = null;
                    else
                        robot.CurrentGoal = goal;

                    iter = --taskEventIterator[i] - 1;
                }
            }
            simulationData.Step--;
            OnTaskFinished(robotOperations);
        }

        public void SetPosition(int step)
        {
            //doesn't handle backward
            if (step < simulationData.Step)
                throw new SimulationStepOutOfRangeException();

            step = Math.Min(step, loadLogDataAccess.GetStepCount());

            RobotOperation[] robotOperations = null!;
            while (simulationData.Step < step)
            {
                robotOperations = loadLogDataAccess.GetRobotOperations(simulationData.Step);

                for (int i = 0; i < simulationData.Robots.Count; i++)
                {
                    Robot robot = simulationData.Robots[i];
                    robot.NextOperation = robotOperations[i];
                    robot.ExecuteMove();
                }

                simulationData.Step++;
            }

            for (int i = 0; i < taskEventIterator.Length; i++)
            {
                var robot = simulationData.Robots[i];

                taskEventIterator[i] = Math.Max(taskEventIterator[i], 0);
                var iter = taskEventIterator[i];

                Goal? currentGoal = robot.CurrentGoal;
                while (iter < taskEvents[i].Length && taskEvents[i][iter].step <= simulationData.Step)
                {
                    TaskEvent taskEvent = taskEvents[i][iter];
                    Goal goal = simulationData.Goals[taskEvent.taskId];
                    if (taskEvent.eventType == TaskEventType.assigned)
                        currentGoal = goal;
                    else
                        currentGoal = null;

                    iter = ++taskEventIterator[i];
                }
                robot.CurrentGoal = currentGoal;
            }
        }

        public void JumpToEnd()
        {
            SetPosition(int.MaxValue);
        }

        public int GetSimulationLength()
        {
            return loadLogDataAccess.GetStepCount();
        }

        #endregion
    }
}
