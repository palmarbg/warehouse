using Persistence.DataTypes;
using Persistence.Extensions;
using Persistence.Interfaces;
using System.Collections.Generic;
using System.Reflection.Metadata;
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
        private static List<List<object>> GetErrors(Log log)
        {
            List<List<object>> err = new List<List<object>>();
            foreach (var error in log.Errors)
            {
                err.Add(new([
                    error.robotId1,
                    error.robotId2,
                    error.round,
                    error.errorType.ToString()
                    ]));
            }
            return err;
        }
        private static List<List<int>> GetTasks(Log log)
        {
            List<List<int>> t = new();
            foreach (var goal in log.Tasks)
            {
                t.Add(new([
                    goal.Id,
                    goal.Position.Y,
                    goal.Position.X //it has to be in this order!!!
                ]));
            }
            return t;
        }
        private static List<List<List<object>>> GetEvents(Log log)
        {
            List<List<List<object>>> e = new();
            for (int i = 0; i < log.Events.Count; i++)
            {
                e.Add(new());
                foreach (var evnt in log.Events[i])
                {
                    e[i].Add(new([
                        evnt.taskId,
                        evnt.step,
                        evnt.eventType.ToString()
                    ]));
                }
            }
            return e;
        }


        private static List<List<object>> GetStart(Log log)
        {
            List<List<object>> s = new();
            foreach (var robotState in log.Start)
            {
                s.Add(new([
                    robotState.X,
                    robotState.Y,
                    robotState.Rotation.ToChar()
                ]));
            }
            return s;
        }
        private static List<string> GetActualPaths(Log log)
        {
            List<string> paths = new();
            foreach (var robotPathList in log.ActualPaths)
            {
                string str = "";
                foreach (var robotPath in robotPathList)
                {
                    str += robotPath.ToChar() + ",";
                }
                if (str.Length > 0)
                    str = str.Remove(str.Length - 1);
                paths.Add(str);
            }
            return paths;
        }
        private static List<string> GetPlannedPaths(Log log)
        {
            List<string> paths = new();
            foreach (var robotPathList in log.PlannerPaths)
            {
                string str = "";
                foreach (var robotPath in robotPathList)
                {
                    str += robotPath.ToChar() + ",";
                }
                if (str.Length > 0)
                    str = str.Remove(str.Length - 1);
                paths.Add(str);
            }
            return paths;
        }

        public void SaveLogData(string path, Log log)
        {

            ExternalLog externalLog = new ExternalLog
            {
                ActionModel = log.ActionModel,
                AllValid = log.AllValid,
                TeamSize = log.TeamSize,
                NumTaskFinished = log.NumTaskFinished,
                SumOfCost = log.SumOfCost,
                MakeSpan = log.MakeSpan,
                ActualPaths = GetActualPaths(log),
                PlannerPaths = GetPlannedPaths(log),
                PlannerTimes = log.PlannerTimes,
                Events = GetEvents(log),
                Tasks = GetTasks(log),
                Errors = GetErrors(log),
                Start = GetStart(log)
            };

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new JsonStringEnumConverter());

            string fileName = path;
            string jsonString = JsonSerializer.Serialize(externalLog, options) ?? throw new NotImplementedException("Serialization of config file was unsuccesful!");
            File.WriteAllText(fileName, jsonString);
        }
    }
}
