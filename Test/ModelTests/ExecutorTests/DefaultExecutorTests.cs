using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Persistence.DataAccesses;
using Persistence.DataTypes;
using Persistence.Interfaces;
using Model.Executors;
using Persistence.Loggers;

namespace Test.ModelTests.ExecutorTests
{
    [TestClass]
    public class DefaultExecutorTests
    {
        private BasicLogger _logger = null!;
        private SimulationData _simulationData = null!;
        private SaveLogDataAccess _saveLogDataAccess = new SaveLogDataAccess();

        [TestInitialize]
        public void Setup()
        {

            _simulationData = new SimulationData
            {
                Map = new ITile[10, 10],
                Goals = new List<Goal>(),
                Robots = new List<Robot>
{
    new Robot { Id = 1, Position = new Position { X = 0, Y = 0 }, Rotation = Direction.Up, NextOperation = RobotOperation.Forward },
    new Robot { Id = 2, Position = new Position { X = 1, Y = 0 }, Rotation = Direction.Up, NextOperation = RobotOperation.Forward },
    new Robot { Id = 3, Position = new Position { X = 2, Y = 0 }, Rotation = Direction.Up, NextOperation = RobotOperation.Forward }
},
                DistributorName = "TestDistributor",
                ControllerName = "TestController",
                RevealedTaskCount = 1
            };
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _simulationData.Map[i, j] = EmptyTile.Instance;
                }
            }

            _logger = new BasicLogger(_simulationData, _saveLogDataAccess);
        }

        [TestMethod]
        public void ExecuteOperationsBasicTests()
        {
            // Arrange
            var executor = new DefaultExecutor(_simulationData, _logger);
            var robot1 = new Robot { Id = 1, Position = new Position { X = 0, Y = 0 }, Rotation = Direction.Down, NextOperation = RobotOperation.Forward };
            var robot2 = new Robot { Id = 2, Position = new Position { X = 1, Y = 1 }, Rotation = Direction.Up, NextOperation = RobotOperation.Clockwise };
            var robot3 = new Robot { Id = 3, Position = new Position { X = 3, Y = 3 }, Rotation = Direction.Up, NextOperation = RobotOperation.Wait };
            _simulationData.Robots[0] = robot1;
            _simulationData.Robots[1] = robot2;
            _simulationData.Robots[2] = robot3;
            var robotOperations = new RobotOperation[] { RobotOperation.Forward, RobotOperation.Clockwise, RobotOperation.Wait };

            // Act
            Assert.IsNotNull(robotOperations);
            var executedOperations = executor.ExecuteOperations(robotOperations, 1.0f);

            // Assert
            // Verify that the robots have moved or rotated correctly based on the operations
            Assert.AreEqual(new Position { X = 0, Y = 1 }, robot1.Position, "Robot 1 should move forward from (0,0) to (0,1)");
            Assert.AreEqual(Direction.Right, robot2.Rotation, "Robot 2 should rotate clockwise from Up to Right");
            Assert.AreEqual(new Position { X = 3, Y = 3 }, robot3.Position, "Robot 3 should not move and remain at (3,3)");
            // Verify output
            Assert.IsNotNull(executedOperations, "Executed operations array should not be null");
            Assert.AreEqual(robotOperations.Length, executedOperations.Length, "Length of executed operations array should match input array");
            CollectionAssert.AreEqual(robotOperations, executedOperations, "Executed operations should match input operations");

        }
        [TestMethod]
        public void ExecuteOperationsCollisionDetectTests()
        {
            var g1 = new Goal() { Id = 1, Position = new Position { X = 0, Y = 5 } };
            var g2 = new Goal() { Id = 2, Position = new Position { X = 5, Y = 0 } };

            // Create two robots, where the second robot blocks the movement of the first
            var robot1 = new Robot { Id = 1, Position = new Position { X = 0, Y = 0 }, Rotation = Direction.Down, NextOperation = RobotOperation.Forward,CurrentGoal = g1 };
            var robot2 = new Robot { Id = 2, Position = new Position { X = 0, Y = 1 }, Rotation = Direction.Up, NextOperation = RobotOperation.Forward,CurrentGoal = g2 };
            var robot3 = new Robot { Id = 3, Position = new Position { X = 3, Y = 3 }, Rotation = Direction.Up, NextOperation = RobotOperation.Wait };

            _simulationData.Robots[0] = robot1;
            _simulationData.Robots[1] = robot2;
            _simulationData.Robots[2] = robot3;
            _simulationData.Map[0, 0] = robot1;
            _simulationData.Map[0, 1] = robot2;
            _simulationData.Map[3, 3] = robot3;

            // Arrange
            var executor = new DefaultExecutor(_simulationData, _logger);
            
            // Both robots have forward operation planned
            var robotOperations = new RobotOperation[] { RobotOperation.Forward, RobotOperation.Forward, RobotOperation.Forward };
            executor.ExecuteOperations(robotOperations,1.0f);

            Assert.AreEqual(false, robot1.MovedThisTurn);
            Assert.AreEqual(false, robot1.BlockedThisTurn);
            Assert.AreEqual(true, robot1.InspectedThisTurn);

            Assert.AreEqual(true,  robot2.MovedThisTurn);
            Assert.AreEqual(false, robot2.BlockedThisTurn);
            Assert.AreEqual(true, robot2.InspectedThisTurn);

            Assert.AreEqual(true, robot3.MovedThisTurn);
            Assert.AreEqual(false, robot3.BlockedThisTurn);
            Assert.AreEqual(true, robot3.InspectedThisTurn);
        }
    }
}
