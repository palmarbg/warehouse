using RobotokModel.Model.Interfaces;
using RobotokModel.Persistence;
using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Mediators.ReplayMediatorUtils
{
    public class ReplayController : IController
    {
        private readonly ILoadLogDataAccess loadLogDataAccess = null!;
        private SimulationData simulationData = null!;
        private TaskEvent[] taskEvents;
        private int taskEventIterator = 0;
        public string Name => "ReplayController";

        public event EventHandler<IControllerEventArgs>? FinishedTask;

        public ReplayController(ILoadLogDataAccess loadLogDataAccess)
        {
            this.loadLogDataAccess = loadLogDataAccess;
            taskEvents = loadLogDataAccess.GetTaskEvents();
        }

        public void CalculateOperations(TimeSpan timeSpan)
        {
            while(taskEventIterator < taskEvents.Length && taskEvents[taskEventIterator].step <= simulationData.Step)
            {
                TaskEvent taskEvent = taskEvents[taskEventIterator];
                Goal goal = simulationData.Goals[taskEvent.taskId];
                goal.IsAssigned = taskEvent.eventType == TaskEventType.assigned;
                Goal.OnGoalsChanged();
                taskEventIterator++;
            }
            OnTaskFinished(loadLogDataAccess.GetRobotOperations(simulationData.Step));
        }

        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan, ITaskDistributor distributor)
        {
            this.simulationData = simulationData;
        }

        public IController NewInstance()
        {
            return new ReplayController(loadLogDataAccess);
        }

        private void OnTaskFinished(RobotOperation[] result)
        {
            FinishedTask?.Invoke(this, new(result));
        }
    }
}
