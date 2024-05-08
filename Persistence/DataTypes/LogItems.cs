namespace Persistence.DataTypes
{
    public struct OperationError(int robotId1, int robotId2, int round, OperationErrorType errorType)
    {
        public int robotId1 = robotId1;
        public int robotId2 = robotId2;
        public int round = round;
        public OperationErrorType errorType = errorType;
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

    public struct TaskEvent(int taskId, int step, TaskEventType eventType)
    {
        public int taskId = taskId;
        public int step = step;
        public TaskEventType eventType = eventType;
    }
}
