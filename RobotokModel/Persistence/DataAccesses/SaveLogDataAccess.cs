using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using RobotokModel.Model;
using RobotokModel.Model.Extensions;

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
                    goal.Position.X,
                    goal.Position.Y
                ]));
            }

            externalLog.Events = new();
            for(int i = 0; i < log.Events.Count; i++)
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


            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new JsonStringEnumConverter());


            string fileName = path;
            string jsonString = JsonSerializer.Serialize(externalLog, options) ?? throw new NotImplementedException("Serialization of config file was unsuccesful!");
            File.WriteAllText(fileName, jsonString);
        }
    }
}
