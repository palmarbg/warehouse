using Model.Executors;
using Persistence.DataAccesses;
using Persistence.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.MockClasses.Loggers;

namespace Test.ModelTests.ExecutorTests.StrictExecutorTests
{
    [TestClass]
    public class LoopTests
    {
        private SimulationData _simulationData = null!;
        private RobotOperation[] _forwardOperations = null!;
        private RobotOperation[] _rotateOperations = null!;
        private List<Robot> _robots = null!;
        private StrictExecutor _executor = null!;
        private MockLogger _logger = null!;

        [TestInitialize]
        public void TestInitLoop_4()
        {
            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("Test"));
            Uri baseUri = new Uri(path);
            Uri fileUri = new Uri(baseUri, "Test/TestFiles/StrictExecutor/configs/1.json");
            string filePath = fileUri.AbsolutePath;
            
            Assert.IsTrue(File.Exists(filePath));

            ConfigDataAccess configDataAccess = new(filePath, new DirectoryDataAccess());

            _simulationData = configDataAccess.GetInitialSimulationData();

            _robots = _simulationData.Robots;

            _robots[0].Rotation = Direction.Down;
            _robots[1].Rotation = Direction.Left;
            _robots[2].Rotation = Direction.Right;
            _robots[3].Rotation = Direction.Up;

            /**
             * DL
             * RU
             */

            _forwardOperations = new RobotOperation[]
            {
                RobotOperation.Forward,
                RobotOperation.Forward,
                RobotOperation.Forward,
                RobotOperation.Forward,
            };

            _rotateOperations = new RobotOperation[]
            {
                RobotOperation.Clockwise,
                RobotOperation.Clockwise,
                RobotOperation.Clockwise,
                RobotOperation.Clockwise,
            };

            _logger = new MockLogger(_simulationData);
            _executor = new StrictExecutor(_simulationData, _logger);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _executor.Dispose();
        }

        [TestMethod]
        public void TestLoop_OneStep()
        {
            float customTime = (float)1 / TimeSpan.FromMilliseconds(123).Milliseconds;
            _executor.ExecuteOperations(_forwardOperations, customTime);
            
            Assert.AreEqual(1, _logger.Steps.Count);
            var log = _logger.Steps[0];

            Assert.AreEqual(0, log.errors.Length);
            Assert.AreEqual(customTime, log.timeElapsed);
            for( int i = 0; i < _robots.Count; i++)
            {
                Assert.AreEqual(_forwardOperations[i], log.controllerOperations[i]);
                Assert.AreEqual(_forwardOperations[i], log.robotOperations[i]);
            }

            Assert.AreEqual(_robots[0].Position, new Position { X = 1, Y = 2 });
            Assert.AreEqual(_robots[1].Position, new Position { X = 1, Y = 1 });
            Assert.AreEqual(_robots[2].Position, new Position { X = 2, Y = 2 });
            Assert.AreEqual(_robots[3].Position, new Position { X = 2, Y = 1 });
        }
    }
}
