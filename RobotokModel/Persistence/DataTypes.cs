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
    public enum Strategy
    {
        RoundRobin,
        AStar,
    }
}
