using Persistence.DataAccesses;
using Persistence.DataTypes;
using Persistence.Loggers;
using System;
using System.Collections.Generic;

namespace Test.PersistenceTests.LoggerTests
{
    [TestClass]
    public class AgentTest
    {
        private DirectoryDataAccess _dirAccess = null!;
        private ConfigDataAccess _configDataAccess = null!;
        private string _files = null!;
        private SimulationData data = null!;
        private BasicLogger logger = null!;
        private SaveLogDataAccess _saveLogDataAccess = null!;

        [TestInitialize]
        public void Initialize()
        {
            _dirAccess = new DirectoryDataAccess();
            string path1 = System.IO.Directory.GetCurrentDirectory().Split("/bin")[0] + "/Files/";
            string path2 = System.IO.Directory.GetCurrentDirectory().Split("\\bin")[0] + "/Files/";
            _files = path1.Length < path2.Length ? path1 : path2;
            _configDataAccess = new ConfigDataAccess(_files + "Config12.json", _dirAccess);
            data = _configDataAccess.GetInitialSimulationData();
            _saveLogDataAccess = new SaveLogDataAccess();
            logger = new BasicLogger(data, _saveLogDataAccess);
        }
        [TestMethod]
        public void LogTimeOutTests()
        {
            Log l = logger.GetLog();
            data.Step = 3;
            Assert.AreEqual(0,l.Errors.Count);
            Assert.AreEqual(20, l.TeamSize);
            Assert.AreEqual(20, l.PlannerPaths.Count);
            Assert.AreEqual(20, l.ActualPaths.Count);
            Assert.AreEqual(0, l.PlannerPaths[0].Count);
            Assert.AreEqual(0, l.ActualPaths[0].Count);
            logger.LogTimeout();
            Assert.AreEqual(1, logger.GetLog().Errors.Count);
            Assert.AreEqual(20, l.PlannerPaths.Count);
            Assert.AreEqual(20, l.ActualPaths.Count);
            Assert.AreEqual(1, l.PlannerPaths[0].Count);
            Assert.AreEqual(1, l.ActualPaths[0].Count);
            Assert.AreEqual(3, l.Errors[0].round);
            Assert.AreEqual(OperationErrorType.timeout, l.Errors[0].errorType);
            Assert.AreEqual(-1, l.Errors[0].robotId1);
            Assert.AreEqual(-1, l.Errors[0].robotId2);
            Assert.AreEqual(RobotOperation.Timeout, l.PlannerPaths[0][0]);
            Assert.AreEqual(RobotOperation.Wait, l.ActualPaths[0][0]);
        }
        [TestMethod]
        public void LogEventTests()
        {
            Log l = logger.GetLog();
            TaskEvent te = new(1, 2, TaskEventType.assigned);
            int robotID = 19;
            logger.LogEvent(te, robotID);
            Assert.AreEqual(te, l.Events[19][0]);
        }
        [TestMethod]
        public void FinalizeLogTests()
        {
            Log l = logger.GetLog();
            float timeElapsed = 0.123F;

            logger.LogStep(
                [RobotOperation.Forward, RobotOperation.Backward], 
                [RobotOperation.Clockwise, RobotOperation.CounterClockwise],
                [new OperationError(1,2,3,OperationErrorType.timeout)],
                timeElapsed);
            Assert.AreEqual(0.123F, l.PlannerTimes[0]);
            Assert.AreEqual(RobotOperation.Forward, l.PlannerPaths[0][0]);
            Assert.AreEqual(RobotOperation.Clockwise, l.ActualPaths[0][0]);
            Assert.AreEqual(RobotOperation.Backward, l.PlannerPaths[1][0]);
            Assert.AreEqual(RobotOperation.CounterClockwise, l.ActualPaths[1][0]);
        }
    }
}