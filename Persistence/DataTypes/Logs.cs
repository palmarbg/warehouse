namespace Persistence.DataTypes
{
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
