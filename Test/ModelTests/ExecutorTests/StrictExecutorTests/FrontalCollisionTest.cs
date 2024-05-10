using Model.Executors;
using Persistence.DataAccesses;
using Persistence.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.MockClasses.Loggers;
using Test.Utils;

namespace Test.ModelTests.ExecutorTests.StrictExecutorTests
{
    [TestClass]
    public class FrontalCollisionTest : TestByLogClassBase
    {
        private StrictExecutor _executor = null!;
        private ExecutorTestLogger _logger = null!;

        [TestInitialize]
        public void TestInit()
        {
            TestInitBase("StrictExecutor", "configs/3.json");

            _robots[0].Rotation = Direction.Right;
            _robots[1].Rotation = Direction.Left;

            _logger = new ExecutorTestLogger(_simulationData);
            _executor = new StrictExecutor(_simulationData, _logger);
        }

        [TestMethod]
        public void Test()
        {
            var operations = Enumerable.Repeat(RobotOperation.Forward, 1).AssignToRobots(_robots).ToArray();

            _executor.ExecuteOperations(operations, 0);

            string filePath = new Uri(_uriBase, "out/3.testlog").AbsolutePath;
            _logger.SaveToFile(filePath);

            TestResult(filePath, "tests/3.test");
        }


    }
}
