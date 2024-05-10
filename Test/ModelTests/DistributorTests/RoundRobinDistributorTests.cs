using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Distributors;
using Moq;
using Persistence.DataAccesses;
using Persistence.DataTypes;

namespace Test.ModelTests.DistributorTests
{
    [TestClass]
    public class RoundRobinDistributorTests
    {
        [TestMethod]
        public void AVailableGoalTests()
        {
            // Arrange
            var simulationData = new SimulationData
            {
                Goals = new List<Goal>
                {
                    new Goal { Id = 1, Position = new Position(){X = 0, Y = 0}, IsAssigned = false }, // Available goal
                    new Goal { Id = 2, Position = new Position(){X = 0, Y = 0},IsAssigned = true },  // Assigned goal
                    new Goal { Id = 3, Position = new Position(){X = 0, Y = 0},IsAssigned = false }  // Available goal
                },
                Map = null!,
                RevealedTaskCount = 1,
                Robots = null!,
                DistributorName = "roundrobin"
            };
            var distributor = new RoundRobinDistributor(simulationData);
            var robot = new Robot { Id = 1, Position = new Position { X = 1, Y = 1 }, Rotation = Direction.Up, NextOperation = RobotOperation.Forward, CurrentGoal = null };
            Assert.IsFalse(distributor.AllTasksAssigned);
            Assert.IsNull(robot.CurrentGoal);
            distributor.AssignNewTask(robot);
            Assert.IsNotNull(robot.CurrentGoal);
            Assert.IsTrue(robot.CurrentGoal.IsAssigned);
            Assert.AreEqual(1, robot.CurrentGoal.Id);
            Assert.IsFalse(distributor.AllTasksAssigned);
            distributor.AssignNewTask(robot);
            Assert.IsTrue(robot.CurrentGoal.IsAssigned);
            Assert.AreEqual(3,robot.CurrentGoal.Id);
            Assert.IsTrue(distributor.AllTasksAssigned);
        }
        [TestMethod]
        public void NoMoreGoalsTests()
        {
            var simulationData = new SimulationData
            {
                Goals = new List<Goal>
                {
                    new Goal { Id = 2, Position = new Position(){X = 0, Y = 0},IsAssigned = true },  // Assigned goal
                },
                Map = null!,
                RevealedTaskCount = 1,
                Robots = null!,
                DistributorName = "roundrobin"
            };
            var distributor = new RoundRobinDistributor(simulationData);
            var robot = new Robot { Id = 1, Position = new Position { X = 1, Y = 1 }, Rotation = Direction.Up, NextOperation = RobotOperation.Forward, CurrentGoal = null };
            Assert.IsNull(robot.CurrentGoal);
            distributor.AssignNewTask(robot);
            Assert.IsNull(robot.CurrentGoal);
            Assert.IsTrue(distributor.AllTasksAssigned);
        }
        [TestMethod]
        public void NoGoalsTests()
        {
            var simulationData = new SimulationData
            {
                Goals = new List<Goal>(),
                Map = null!,
                RevealedTaskCount = 1,
                Robots = null!,
                DistributorName = "roundrobin"
            };
            var robot = new Robot { Id = 1, Position = new Position { X = 1, Y = 1 }, Rotation = Direction.Up, NextOperation = RobotOperation.Forward, CurrentGoal = null };
            Assert.IsNull(robot.CurrentGoal);
            var distributor = new RoundRobinDistributor(simulationData);
            distributor.AssignNewTask(robot);
            Assert.IsNull(robot.CurrentGoal);
            Assert.IsTrue(distributor.AllTasksAssigned);
        }
    }
}
