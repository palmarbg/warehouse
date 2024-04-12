using RobotokModel.Model;
using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence.DataAccesses
{
    public class MockLoadLogDataAccess : ILoadLogDataAccess
    {
        #region Private fields
        
        private readonly DemoDataAccess _demoDataAccess;
        private readonly List<RobotOperation[]> _robotOperations;
        private readonly TaskEvent[] _taskEvents;

        #endregion
        public MockLoadLogDataAccess()
        {
            _demoDataAccess = new DemoDataAccess("");

            RobotOperation[] robotOperations = Enumerable.Repeat<RobotOperation>(RobotOperation.Forward, 3).ToArray();
            _robotOperations = Enumerable.Repeat(robotOperations, 15).ToList();

            _taskEvents = new TaskEvent[0];
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

        public TaskEvent[] GetTaskEvents()
        {
            TaskEvent[] taskEvents = new TaskEvent[_taskEvents.Length];
            Array.Copy(_taskEvents, taskEvents, _taskEvents.Length);
            return taskEvents;
        }

        public IDataAccess NewInstance(string filePath)
        {
            return new MockLoadLogDataAccess();
        }
    }
}
