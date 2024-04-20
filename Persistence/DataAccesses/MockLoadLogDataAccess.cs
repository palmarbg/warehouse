using Persistence.DataTypes;
using Persistence.Interfaces;

namespace Persistence.DataAccesses
{
    public class MockLoadLogDataAccess : ILoadLogDataAccess
    {
        #region Private fields

        private readonly DemoDataAccess _demoDataAccess;
        private readonly List<RobotOperation[]> _robotOperations;
        private readonly List<TaskEvent[]> _taskEvents;

        #endregion
        public MockLoadLogDataAccess()
        {
            _demoDataAccess = new DemoDataAccess("");

            RobotOperation[] robotOperations = Enumerable.Repeat<RobotOperation>(RobotOperation.Forward, 3).ToArray();
            robotOperations = [
                RobotOperation.Forward, RobotOperation.Clockwise, RobotOperation.CounterClockwise
                ];
            _robotOperations = Enumerable.Repeat(robotOperations, 15).ToList();

            _taskEvents = [[new(0, 0, TaskEventType.assigned)], [], []];
        }
        public SimulationData GetInitialSimulationData()
        {
            return _demoDataAccess.GetInitialSimulationData();
        }

        public RobotOperation[] GetRobotOperations(int step)
        {
            RobotOperation[] robotOperations = new RobotOperation[_robotOperations[step].Length];
            Array.Copy(_robotOperations[step], robotOperations, _robotOperations[step].Length);
            return robotOperations;
        }

        public List<TaskEvent[]> GetTaskEvents()
        {
            List<TaskEvent[]> toreturn = new List<TaskEvent[]>();
            for (int i = 0; i < _taskEvents.Count; i++)
            {
                TaskEvent[] taskEvents = new TaskEvent[_taskEvents[i].Length];
                Array.Copy(_taskEvents[i], taskEvents, _taskEvents[i].Length);
                toreturn.Add(taskEvents);
            }

            return toreturn;
        }

        public IDataAccess NewInstance(string filePath)
        {
            return new MockLoadLogDataAccess();
        }
    }
}
