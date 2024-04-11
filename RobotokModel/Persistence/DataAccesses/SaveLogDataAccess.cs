using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace RobotokModel.Persistence.DataAccesses
{
    public class SaveLogDataAccess : ISaveLogDataAccess
    {
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

            List<string> actualPaths = new List<string>();
            List<string> plannedPaths = new List<string>();


            ExternalLog externalLog = new ExternalLog
            {
                ActionModel = log.ActionModel,
                AllValid = log.AllValid,
                TeamSize = log.TeamSize,
                NumTaskFinished = log.NumTaskFinished,
                SumOfCost = log.SumOfCost,
                MakeSpan = log.MakeSpan,
                ActualPaths = actualPaths,
                PlannerPaths = plannedPaths,
                PlannerTimes = log.PlannerTimes,
                Events = null!,
                Tasks = null!,
                Errors = null!,
                Start = null!
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
