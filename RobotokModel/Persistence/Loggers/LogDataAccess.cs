using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using RobotokModel.Model;

namespace RobotokModel.Persistence.Loggers
{
    class LogDataAccess
    {
        private Uri baseUri = null!;
        public Log LoadLog(string path) // TODO: cleanup, async?
        {
            var cucc = path;
            baseUri = new(path);
            string jsonString = File.ReadAllText(path);
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new JsonStringEnumConverter());
            ExternalLog? externalLog = JsonSerializer.Deserialize<ExternalLog>(jsonString, options) ?? throw new NotImplementedException("Serialization of config file was unsuccesful!");
            Log log = new Log();

            log.ActionModel = externalLog.ActionModel;
            log.AllValid = externalLog.AllValid;
            log.TeamSize = externalLog.TeamSize;
            log.NumTaskFinished = externalLog.NumTaskFinished;
            log.SumOfCost = externalLog.SumOfCost;
            log.MakeSpan = externalLog.MakeSpan;
            log.ActualPaths = new List<List<RobotOperation>>();
            log.PlannerPaths = new List<List<RobotOperation>>();
            foreach (string robot in externalLog.ActualPaths)
            {
                List<RobotOperation> robotOperation = new List<RobotOperation>();
                string[] sep = new string[log.TeamSize];
                sep = robot.Split(',');
                RobotOperation operation;
                foreach (string op in sep)
                {
                    switch (op)
                    {
                        case "F":
                            operation = RobotOperation.Forward;
                            break;
                        case "R":
                            operation = RobotOperation.Clockwise;
                            break;
                        case "C":
                            operation = RobotOperation.CounterClockwise;
                            break;
                        case "W":
                            operation = RobotOperation.Wait;
                            break;
                        default:
                            throw new ArgumentException();
                    }
                    robotOperation.Add(operation);
                }
                log.ActualPaths.Add(robotOperation);
            }
            foreach (string robot in externalLog.PlannerPaths)
            {
                List<RobotOperation> robotOperation = new List<RobotOperation>();
                string[] sep = new string[log.TeamSize];
                sep = robot.Split(',');
                RobotOperation operation;
                foreach (string op in sep)
                {
                    switch (op)
                    {
                        case "F":
                            operation = RobotOperation.Forward;
                            break;
                        case "R":
                            operation = RobotOperation.Clockwise;
                            break;
                        case "C":
                            operation = RobotOperation.CounterClockwise;
                            break;
                        case "W":
                            operation = RobotOperation.Wait;
                            break;
                        case "T":
                            operation = RobotOperation.Timeout;
                            break;
                        default:
                            throw new ArgumentException();
                    }
                    robotOperation.Add(operation);
                }
                log.PlannerPaths.Add(robotOperation);
            }
            log.Events = new List<List<TaskEvent>>();
            foreach (List<List<Object>> round in externalLog.Events)
            {
                List<TaskEvent> taskEvents = new List<TaskEvent>();
                foreach (List<Object> task in round)
                {
                    string? t0 = task[0].ToString();
                    string? t1 = task[1].ToString();
                    if (t0 == null ||t1 == null)
                    {
                        throw new Exception();
                    }

                    int taskID = int.Parse(t0);
                    int robotID = int.Parse(t1);
                    if (task[2].ToString() == "finished")
                    {
                        taskEvents.Add(new TaskEvent(taskID, robotID, TaskEventType.finished));
                    }
                    else
                    {
                        taskEvents.Add(new TaskEvent(taskID, robotID, TaskEventType.assigned));
                    }

                }
                log.Events.Add(taskEvents);
            }
            // TODO: értelmesebb formára hozni a taskokat jelenleg [int,int,int] -> [valami id, position, position]
            log.Tasks = externalLog.Tasks;
            // TODO: robotok kezdeti poz. jobb tárolása [int,int,string] -> [pos, direction]
            log.Start = externalLog.Start;

            // TODO: befejezni
            foreach (List<Object> error in externalLog.Errors)
            {
                string? e0 = error[0].ToString();
                string? e1 = error[1].ToString();
                string? e2 = error[2].ToString();
                if (e0 == null || e1 == null || e2 == null)
                {
                    throw new Exception();
                }

                int robot1 = int.Parse(e0);
                int robot2 = int.Parse(e1);
                int stepID = int.Parse(e2);
                string? errorType = error[2].ToString();
                if(errorType == null)
                {
                    throw new Exception();
                }

            }
            log.Errors = null!;

            return log;
        }

        public Task SaveLog(string path)
        {
            throw new NotImplementedException();
        }
        public void LogTimeout()
        {
            throw new NotImplementedException();
        }
        public void LogEvent(TaskEvent taskEvent)
        {
            throw new NotImplementedException();
        }
        public void LogStep(
        RobotOperation[] controllerOperations,
        RobotOperation[] robotOperations,
        OperationError[] errors
        )
        {
            throw new NotImplementedException();
        }
    }
}
