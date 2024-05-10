using Model.Executors;
using Model.Interfaces;
using Persistence.DataAccesses;
using Persistence.DataTypes;
using Persistence.Loggers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Test.MockClasses.Loggers;
using Test.Utils;

namespace Test.ModelTests.ExecutorTests.StrictExecutorTests
{
    [TestClass]
    public class ChainTest : TestByLogClassBase
    {
        private StrictExecutor _executor = null!;
        private ExecutorTestLogger _logger = null!;


        [TestInitialize]
        public void TestInit()
        {
            TestInitBase("StrictExecutor", "configs/2.json");
            
            Enumerable.Repeat(Direction.Right, 4).AssignToRobots(_robots);

            _logger = new ExecutorTestLogger(_simulationData);
            _executor = new StrictExecutor(_simulationData, _logger);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _executor.Dispose();
        }

        [TestMethod]
        public void TestOneStep()
        {
            float customTime = (float)1 / TimeSpan.FromMilliseconds(123).Milliseconds;
            var operations = Enumerable.Repeat(RobotOperation.Forward, 1).AssignToRobots(_robots).ToArray();

            _executor.ExecuteOperations(operations, customTime);

            string filePath = new Uri(_uriBase, "out/1.testlog").AbsolutePath;
            _logger.SaveToFile(filePath);

            string testPath = new Uri(_uriBase, "tests/1.test").AbsolutePath;
            Assert.IsTrue(AreFilesEqual(filePath, testPath));
        }

        [TestMethod]
        public void TestMoreSteps()
        {
            float customTime = (float)1 / TimeSpan.FromMilliseconds(123).Milliseconds;
            
            for (int i = 0; i < 4; i++)
            {
                var operations = Enumerable.Repeat(RobotOperation.Forward, 1).AssignToRobots(_robots).ToArray();
                _executor.ExecuteOperations(operations, customTime);
            }

            string filePath = new Uri(_uriBase, "out/2.testlog").AbsolutePath;
            _logger.SaveToFile(filePath);

            TestResult(filePath, "tests/2.test");
        }


    }
}
