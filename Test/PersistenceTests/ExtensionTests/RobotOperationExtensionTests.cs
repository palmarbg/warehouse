using Persistence.DataTypes;
using Persistence.Extensions;

namespace TestModel.PersistenceTests.ExtensionTests
{
    [TestClass]
    public class RobotOperationExtensionTests
    {
        [TestMethod]
        public void ToCharTests()
        {
            Assert.AreEqual('F', RobotOperationExtensions.ToChar(RobotOperation.Forward));
            Assert.AreEqual('R', RobotOperationExtensions.ToChar(RobotOperation.Clockwise));
            Assert.AreEqual('C', RobotOperationExtensions.ToChar(RobotOperation.CounterClockwise));
            Assert.AreEqual('B', RobotOperationExtensions.ToChar(RobotOperation.Backward));
            Assert.AreEqual('W', RobotOperationExtensions.ToChar(RobotOperation.Timeout));
            Assert.AreEqual('W', RobotOperationExtensions.ToChar(RobotOperation.Wait));
        }

        [TestMethod]
        public void ToRobotOperationTests()
        {
            Assert.AreEqual(RobotOperation.Forward, RobotOperationExtensions.ToRobotOperation('F'));
            Assert.AreEqual(RobotOperation.Clockwise, RobotOperationExtensions.ToRobotOperation('R'));
            Assert.AreEqual(RobotOperation.CounterClockwise, RobotOperationExtensions.ToRobotOperation('C'));
            Assert.AreEqual(RobotOperation.Backward, RobotOperationExtensions.ToRobotOperation('B'));
            Assert.AreEqual(RobotOperation.Wait, RobotOperationExtensions.ToRobotOperation('T'));
            Assert.AreEqual(RobotOperation.Wait, RobotOperationExtensions.ToRobotOperation('W'));
        }

        [TestMethod]
        public void ReverseTests()
        {
            Assert.AreEqual(RobotOperation.Forward, RobotOperationExtensions.Reverse(RobotOperation.Backward));
            Assert.AreEqual(RobotOperation.Backward, RobotOperationExtensions.Reverse(RobotOperation.Forward));
            Assert.AreEqual(RobotOperation.Clockwise, RobotOperationExtensions.Reverse(RobotOperation.CounterClockwise));
            Assert.AreEqual(RobotOperation.CounterClockwise, RobotOperationExtensions.Reverse(RobotOperation.Clockwise));
            Assert.AreEqual(RobotOperation.Wait, RobotOperationExtensions.Reverse(RobotOperation.Wait));
            Assert.AreEqual(RobotOperation.Wait, RobotOperationExtensions.Reverse(RobotOperation.Timeout));
        }
    }
}
