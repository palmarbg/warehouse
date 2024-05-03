using Persistence.DataTypes;
using Persistence.Extensions;

namespace TestModel.PersistenceTests.ExtensionTests
{
    [TestClass]
    public class RobotExtensionTests
    {
        [TestMethod]
        public void ExecuteMoveTest() // Rest of rotation are tested in other extension tests
        {
            Robot r = new() { Id = 0, Position = new() { X = 1, Y = 1}, Rotation = Direction.Up };
            r.NextOperation = RobotOperation.Forward;
            RobotExtensions.ExecuteMove(r);
            Assert.AreEqual(0,r.Position.Y);

            r.NextOperation = RobotOperation.Backward;
            RobotExtensions.ExecuteMove(r);
            Assert.AreEqual(1,r.Position.Y);

            r.NextOperation = RobotOperation.CounterClockwise;
            RobotExtensions.ExecuteMove(r);
            Assert.AreEqual(Direction.Left,r.Rotation);

            r.NextOperation = RobotOperation.Clockwise;
            RobotExtensions.ExecuteMove(r);
            Assert.AreEqual(Direction.Up,r.Rotation);

            r.NextOperation = RobotOperation.Timeout;
            RobotExtensions.ExecuteMove(r);
            Assert.AreEqual(0,r.Id);
            Assert.AreEqual(1,r.Position.X);
            Assert.AreEqual(1,r.Position.Y);
            Assert.AreEqual(Direction.Up,r.Rotation);
        }

    }
}
