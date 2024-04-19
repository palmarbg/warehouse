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
        public OperationError(int robotId1, int robotId2, int round, OperationErrorType errorType)
        {
            this.robotId1 = robotId1;
            this.robotId2 = robotId2;
            this.round = round;
            this.errorType = errorType;
        }
        public int robotId1;
        public int robotId2;
        public int round;
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

    public struct RobotState
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Rotation { get; set; }
    }

        public struct TaskEvent
    {
        public TaskEvent(int taskId, int step, TaskEventType eventType)
        {
            this.taskId = taskId;
            this.step = step;
            this.eventType = eventType;
        }
        public int taskId;
        public int step;
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
        public int NumTaskFinished { get; set; }
        public int SumOfCost { get; set; }
        public int MakeSpan { get; set; }
        public List<List<RobotOperation>> ActualPaths { get; set; } = null!;
        public List<List<RobotOperation>> PlannerPaths { get; set; } = null!;
        public List<float> PlannerTimes { get; set; } = null!;
        public List<List<TaskEvent>> Events { get; set; } = null!;
        public List<Goal> Tasks { get; set; } = null!;
        public List<OperationError> Errors { get; set; } = null!;
        public List<RobotState> Start { get; set; } = null!;
    }    
    public class ExternalLog
    {
        public required string ActionModel { get; set; }
        public required string AllValid { get; set; }
        public required int TeamSize { get; set; }
        public required int NumTaskFinished { get; set; }
        public required int SumOfCost { get; set; }
        public required int MakeSpan { get; set; }
        public required List<string> ActualPaths { get; set; }
        public required List<string> PlannerPaths { get; set; }
        public required List<float> PlannerTimes { get; set; }
        public required List<List<Object>> Errors { get; set; }
        public required List<List<List<Object>>> Events { get; set; }
        public required List<List<Object>> Start { get; set; }
        public required List<List<int>> Tasks { get; set; }
    }

}
