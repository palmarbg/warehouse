namespace Persistence.DataTypes
{
    public class Config
    {
        public required string MapFile { get; set; }
        public required string AgentFile { get; set; }
        public required int TeamSize { get; set; }
        public required string TaskFile { get; set; }
        public required int NumTasksReveal { get; set; }
        public required string TaskAssignmentStrategy { get; set; }
    }
}
