using RobotokModel.Model;
using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence.Loggers
{
    class TestLogger : ILogger
    {
        public void SaveLog(string path)
        {
            throw new NotImplementedException();
        }
        public void LogTimeout()
        {
            throw new NotImplementedException();
        }
        public void LogEvent(TaskEvent taskEvent)
        {
            throw new NotImplementedException();
        }

        public void LogStep(
        RobotOperation[] controllerOperations,
        RobotOperation[] robotOperations,
        OperationError[] errors,
        float timeElapsed
        )
        {
            throw new NotImplementedException();
        }

        public ILogger NewInstance(SimulationData simulationData)
        {
            return new TestLogger();
        }
    }
}
