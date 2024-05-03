using Persistence.DataAccesses;
using Persistence.DataTypes;
using Persistence.Extensions;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.PersistenceTests.DataAccessTests.SaveLogDataAccessTests
{
    [TestClass]
    public class SimpleTest
    {

        private SaveLogDataAccess _saveLogDataAccess = null!;

        [TestInitialize]
        public void Initialize()
        {
            _saveLogDataAccess = new SaveLogDataAccess();
        }

        [TestMethod]
        public void ExternalLogTests()
        {
            Log log = new();
            log.ActionModel = "MAPF_T";
            log.AllValid = "Yes";
            log.TeamSize = 0;
            log.NumTaskFinished = 0;
            log.SumOfCost = 0;
            log.MakeSpan = 0;
            log.ActualPaths = [[RobotOperation.Forward], [RobotOperation.Clockwise], [RobotOperation.CounterClockwise], [RobotOperation.Wait,RobotOperation.Timeout]];
            log.PlannerPaths = [[RobotOperation.Timeout]];
            log.PlannerTimes = new List<float>();
            log.PlannerTimes = [(float)0.123,(float)1.23];
            log.Events = [new()];
            TaskEvent e1 = new TaskEvent(1, 1,TaskEventType.assigned);
            log.Events[0].Add(e1);
            TaskEvent e2 = new TaskEvent(2, 1, TaskEventType.finished);
            log.Events[0].Add(e2);
            Goal g = new() { Id = 1, Position = new Position() { X = 1, Y = 2 }, IsAssigned = false };
            OperationError oe = new(0,1,2,OperationErrorType.collision);
            log.Tasks = [g];
            log.Errors = [oe];
            RobotState rs1 = new() { X = 0, Y = 1, Rotation = Direction.Left};
            RobotState rs2 = new() { X = 0, Y = 1, Rotation = Direction.Right };
            RobotState rs3 = new() { X = 0, Y = 1, Rotation = Direction.Up };
            RobotState rs4 = new() { X = 0, Y = 1, Rotation = Direction.Down };
            log.Start = [rs1,rs2,rs3,rs4];

            _saveLogDataAccess.NewInstance();
            ExternalLog result = _saveLogDataAccess.GetExternalLog(log);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ActionModel, "MAPF_T");
            Assert.AreEqual(result.AllValid, "Yes");
            Assert.AreEqual(result.TeamSize, 0);
            Assert.AreEqual(result.SumOfCost, 0);
            Assert.AreEqual(result.NumTaskFinished, 0);
            Assert.AreEqual(result.MakeSpan, 0);
            Assert.AreEqual(result.PlannerTimes.Count, 2);
            Assert.AreEqual(result.PlannerTimes[0], (float)0.123);
            Assert.AreEqual(result.PlannerTimes[1], (float)1.23);

            Assert.AreEqual(result.ActualPaths.Count, 4);
            Assert.AreEqual(result.ActualPaths[0], "F");
            Assert.AreEqual(result.ActualPaths[1], "R");
            Assert.AreEqual(result.ActualPaths[2], "C");
            Assert.AreEqual(result.ActualPaths[3], "W,W");
            Assert.AreEqual(result.PlannerPaths[0], "W");

            Assert.AreEqual(result.Start.Count, 4);
            Assert.AreEqual(result.Start[0][0], 0);
            Assert.AreEqual(result.Start[0][1], 1);
            Assert.AreEqual(result.Start[0][2], "W");
            Assert.AreEqual(result.Start[1][2], "E");
            Assert.AreEqual(result.Start[2][2], "N");
            Assert.AreEqual(result.Start[3][2], "S");

        }
        
    }
}
