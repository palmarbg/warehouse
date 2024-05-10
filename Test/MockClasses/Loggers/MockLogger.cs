using Persistence.DataTypes;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.MockClasses.Loggers
{
    public class MockLogger(SimulationData simulationData) : ILogger
    {
        private SimulationData _simulationData = simulationData;

        public List<(TaskEvent, int)> TaskEvents = new List<(TaskEvent, int)>();
        public List<Step> Steps = new List<Step> ();

        public void LogEvent(TaskEvent taskEvent, int robotId)
        {
            TaskEvents.Add((taskEvent, robotId));
        }

        public void LogStep(RobotOperation[] controllerOperations, RobotOperation[] robotOperations, OperationError[] errors, float timeElapsed)
        {
            Steps.Add(new Step(controllerOperations, robotOperations, errors, timeElapsed));
        }

        public void LogTimeout()
        {
            throw new NotImplementedException();
        }

        public ILogger NewInstance(SimulationData simulationData)
        {
            throw new NotImplementedException();
        }

        public void SaveLog(string path)
        {
            throw new NotImplementedException();
        }

        public struct Step(RobotOperation[] controllerOpeations, RobotOperation[] robotOperations, OperationError[] errors, float timeElapsed)
        {
            public RobotOperation[] controllerOperations = controllerOpeations;
            public RobotOperation[] robotOperations = robotOperations;
            public OperationError[] errors = errors;
            public float timeElapsed = timeElapsed;
        }
    }
}
