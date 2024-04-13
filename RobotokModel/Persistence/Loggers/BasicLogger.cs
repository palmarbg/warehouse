using RobotokModel.Model;
using RobotokModel.Persistence.DataAccesses;
using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence.Loggers
{
    class BasicLogger : ILogger
    {
        private SaveLogDataAccess dataAccess = new SaveLogDataAccess();
        private Log log = new Log();
        private int round = 0;
        public BasicLogger(string actionModel, int teamSize, SimulationData simulationData)
        {
            // Konstruktorból inicializálva
            log.ActionModel = actionModel;
            log.TeamSize = teamSize;
            log.Start = simulationData.Robots;
            log.Tasks = simulationData.Goals;


            // LogEvent()
            log.Events = new List<List<TaskEvent>>();
            for(int i = 0; i < teamSize; i++)
            {
                log.Events.Add(new List<TaskEvent>());
            }

            // LogStep() vagy LogTimeOut()
            log.PlannerPaths = new List<List<RobotOperation>>();
            log.ActualPaths = new List<List<RobotOperation>>();
            log.Errors = new List<OperationError>();
            log.PlannerTimes = new List<float>();
        }
        public void SaveLog(string path)
        {
            log.AllValid = log.Errors.Count == 0 ? "Yes" : "No";
            log.NumTaskFinished = 0;
            foreach(List<TaskEvent> events in log.Events)
            {
                foreach(TaskEvent v in events)
                {
                    if (v.eventType == TaskEventType.assigned)
                        log.NumTaskFinished += 1;
                }
            }
            log.MakeSpan = log.ActualPaths[0].Count;
            log.SumOfCost = log.ActualPaths.Count * log.ActualPaths[0].Count;
            dataAccess.SaveLogData(path, log);
        }
        public void LogTimeout()
        {
            log.Errors.Add(
                new OperationError
                {
                    robotId1 = -1,
                    robotId2 = -1,
                    round = this.round,
                    errorType = OperationErrorType.timeout
                }
            );

            for (int i = 0; i < log.TeamSize; i++)
            {
                log.PlannerPaths[i].Add(RobotOperation.Timeout);
                log.ActualPaths[i].Add(RobotOperation.Wait);
            }
            round += 1;
        }
        public void LogEvent(TaskEvent taskEvent)
        {
            //log.Events[taskEvent.robotId].Add(taskEvent);
            log.Events[taskEvent.step].Add(taskEvent);
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

            for(int i = 0; i < controllerOperations.Length; i++)
            {
                log.PlannerPaths[i].Add(controllerOperations[i]);
                log.ActualPaths[i].Add(robotOperations[i]);
            }
            log.PlannerTimes.Add(timeElapsed);
            round += 1;
        }
    }
}
