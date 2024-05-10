using Persistence.DataTypes;
using Persistence.Interfaces;
using System.Diagnostics;

namespace Persistence.Loggers
{
    public class BasicLogger : ILogger
    {
        private bool flag = false;
        private ISaveLogDataAccess _saveLogDataAccess;
        protected Log log = new Log();
        private SimulationData simulationData;
        public BasicLogger(SimulationData simulationData, ISaveLogDataAccess saveLogDataAccess)
        {
            //Debug.WriteLine(simulationData.Robots.Count);
            //Debug.WriteLine(simulationData.Robots.Count);
            //Debug.WriteLine(simulationData.Robots.Count);
            //Debug.WriteLine(simulationData.Robots.Count);
            //Debug.WriteLine(simulationData.Robots.Count);


            _saveLogDataAccess = saveLogDataAccess;

            string actionModel = simulationData.ControllerName ?? string.Empty;
            int teamSize = simulationData.Robots.Count;
            this.simulationData = simulationData;
            log.ActionModel = actionModel;
            log.TeamSize = teamSize;

            log.Start = [];

            foreach (var robot in simulationData.Robots)
            {
                log.Start.Add(new RobotState
                {
                    Rotation = robot.Rotation,
                    X = robot.Position.X,
                    Y = robot.Position.Y
                });
            }




            // LogEvent()
            log.Events = new List<List<TaskEvent>>();
            // LogStep() vagy LogTimeOut()
            log.PlannerPaths = new List<List<RobotOperation>>();
            log.ActualPaths = new List<List<RobotOperation>>();
            for (int i = 0; i < teamSize; i++)
            {
                log.Events.Add(new List<TaskEvent>());
                log.PlannerPaths.Add(new List<RobotOperation>());
                log.ActualPaths.Add(new List<RobotOperation>());
            }

            log.Errors = new List<OperationError>();
            log.PlannerTimes = new List<float>();
        }
        public void SaveLog(string path)
        {
            FinalizeLog();
            _saveLogDataAccess.SaveLogData(path, log);
        }
        public void FinalizeLog()
        {
            log.Tasks = simulationData.Goals;
            log.AllValid = !log.Errors.All(e => e.errorType != OperationErrorType.timeout) ? "Yes" : "No";
            log.NumTaskFinished = 0;
            foreach (List<TaskEvent> events in log.Events)
            {
                foreach (TaskEvent v in events)
                {
                    if (v.eventType == TaskEventType.assigned)
                        log.NumTaskFinished += 1;
                }
            }
            log.MakeSpan = log.ActualPaths[0].Count;
            log.SumOfCost = log.ActualPaths.Count * log.ActualPaths[0].Count;
        }
        public Log GetLog()
        {
            return log;
        }
        public void LogTimeout()
        {
            int round = simulationData.Step;
            log.Errors.Add(
                new OperationError
                {
                    robotId1 = -1,
                    robotId2 = -1,
                    round = round,
                    errorType = OperationErrorType.timeout
                }
            );

            for (int i = 0; i < log.TeamSize; i++)
            {
                log.PlannerPaths[i].Add(RobotOperation.Timeout);
                log.ActualPaths[i].Add(RobotOperation.Wait);
            }
        }
        public void LogEvent(TaskEvent taskEvent, int robotId)
        {
            if (flag)
                return;
            try
            {
            log.Events[robotId].Add(taskEvent);

            }
            catch (Exception)
            {

                
            }
        }

        public void LogStep(
        RobotOperation[] controllerOperations,
        RobotOperation[] robotOperations,
        OperationError[] errors,
        float timeElapsed
        )
        {
            foreach (OperationError e in errors)
            {
                log.Errors.Add(e);
            }

            for (int i = 0; i < controllerOperations.Length; i++)
            {
                log.PlannerPaths[i].Add(controllerOperations[i]);
                log.ActualPaths[i].Add(robotOperations[i]);
            }
            log.PlannerTimes.Add(timeElapsed);
        }

        public ILogger NewInstance(SimulationData simulationData)
        {
            flag = true;
            return new BasicLogger(simulationData, _saveLogDataAccess.NewInstance());
        }
    }
}
