using Persistence.DataTypes;
using Persistence.Extensions;
using Persistence.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Persistence.DataAccesses
{
    public class SaveLogDataAccess : ISaveLogDataAccess
    {
        public ISaveLogDataAccess NewInstance()
        {
            return new SaveLogDataAccess();
        }
        public void SaveLogData(string path, Log log)
        {
            /*
        public List<List<RobotOperation>> ActualPaths { get; set; } = null!;
        public List<List<RobotOperation>> PlannerPaths { get; set; } = null!;

        public List<List<TaskEvent>> Events { get; set; } = null!;
        public List<Goal> Tasks { get; set; } = null!;
        public List<OperationError> Errors { get; set; } = null!;
        public List<Robot> Start { get; set; } = null!;
             */

            ExternalLog externalLog = new ExternalLog
            {
                ActionModel = log.ActionModel,
                AllValid = log.AllValid,
                TeamSize = log.TeamSize,
                NumTaskFinished = log.NumTaskFinished,
                SumOfCost = log.SumOfCost,
                MakeSpan = log.MakeSpan,
                ActualPaths = null!,
                PlannerPaths = null!,
                PlannerTimes = log.PlannerTimes,
                Events = null!,
                Tasks = null!,
                Errors = null!,
                Start = null!
            };

            externalLog.Errors = new List<List<object>>();
            foreach (var error in log.Errors)
            {
                externalLog.Errors.Add(new([
                    error.robotId1,
                    error.robotId2,
                    error.round,
                    error.errorType.ToString()
                    ]));
            }

            externalLog.Tasks = new();
            foreach (var goal in log.Tasks)
            {
                externalLog.Tasks.Add(new([
                    goal.Id,
                    goal.Position.Y,
                    goal.Position.X //it has to be in this order!!!
                ]));
            }

            externalLog.Events = new();
            for (int i = 0; i < log.Events.Count; i++)
            {
                externalLog.Events.Add(new());
                foreach (var evnt in log.Events[i])
                {
                    externalLog.Events[i].Add(new([
                        evnt.taskId,
                        evnt.step,
                        evnt.eventType.ToString()
                    ]));
                }
            }

            externalLog.Start = new();
            foreach (var robotState in log.Start)
            {
                externalLog.Start.Add(new([
                    robotState.X,
                    robotState.Y,
                    robotState.Rotation.ToChar()
                ]));
            }

            externalLog.ActualPaths = new();
            foreach (var robotPathList in log.ActualPaths)
            {
                string str = "";
                foreach (var robotPath in robotPathList)
                {
                    str += robotPath.ToChar() + ",";
                }
                if(str.Length > 0)
                    str = str.Remove(str.Length - 1);
                externalLog.ActualPaths.Add(str);
            }

            externalLog.PlannerPaths = new();
            foreach (var robotPathList in log.PlannerPaths)
            {
                string str = "";
                foreach (var robotPath in robotPathList)
                {
                    str += robotPath.ToChar() + ",";
                }
                if(str.Length > 0)
                    str = str.Remove(str.Length - 1);
                externalLog.PlannerPaths.Add(str);
            }


            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new JsonStringEnumConverter());


            string fileName = path;
            string jsonString = JsonSerializer.Serialize(externalLog, options) ?? throw new NotImplementedException("Serialization of config file was unsuccesful!");
            File.WriteAllText(fileName, jsonString);
        }
    }
}
