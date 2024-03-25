using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence
{
    public struct OperationError
    {
        public int robotId1;
        public int robotId2;
        public OperationErrorType errorType;
    }

    public enum OperationErrorType
    {
        timeout, wallhit, collision
    }

    public enum TaskEventType
    {
        assigned, finished
    }

    public struct TaskEvent
    {
        public int taskId;
        public int robotId;
        public TaskEventType eventType;
    }

    public class Config
    {
        public required string MapFile { get; set; }
        public required string AgentFile { get; set; }
        public required int TeamSize { get; set; }
        public required string TaskFile { get; set; }
        public required int NumTasksReveal { get; set; }
        public required string TaskAssignmentStrategy { get; set; }
    }

    public class Log
    {
        public string ActionModel { get; set; } = null!;
        public string AllValid { get; set; } = null!;
        public int TeamSize { get; set; }
        public List<List<Object>> Start { get; set; } = null!;
        public int NumTaskFinished { get; set; }
        public int SumOfCost { get; set; }
        public int MakeSpan { get; set; }
        public List<string> ActualPaths { get; set; } = null!;
        public List<string> PlannerPaths { get; set; } = null!;
        public List<float> PlannerTimes { get; set; } = null!;
        public List<Object> Errors { get; set; } = null!;
        public List<List<List<Object>>> Events { get; set; } = null!;
        public List<List<int>> Tasks { get; set; } = null!;

    }

}
