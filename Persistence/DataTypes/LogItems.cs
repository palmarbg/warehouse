namespace Persistence.DataTypes
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
}
