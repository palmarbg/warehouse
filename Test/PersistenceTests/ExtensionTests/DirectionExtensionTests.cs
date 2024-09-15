using Persistence.DataTypes;
using Persistence.Extensions;

namespace TestModel.PersistenceTests.ExtensionTests
{
    [TestClass]
    public class DirectionExtensionTests
    {
        [TestMethod]
        public void RotateClockwiseTest()
        {
            Assert.AreEqual(
                Direction.Right,
                Direction.Up.RotateClockWise()
                );

            Assert.AreEqual(
                Direction.Down,
                Direction.Right.RotateClockWise()
                );

            Assert.AreEqual(
                Direction.Left,
                Direction.Down.RotateClockWise()
                );

            Assert.AreEqual(
                Direction.Up,
                Direction.Left.RotateClockWise()
                );
        }

        [TestMethod]
        public void ToDirectionTest()
        {
            Assert.AreEqual(
                Direction.Up,
                "N".ToDirection()
                );

            Assert.AreEqual(
                 Direction.Right,
                 "E".ToDirection()
                 );

            Assert.AreEqual(
                Direction.Down,
                "S".ToDirection()
                );

            Assert.AreEqual(
                Direction.Left,
                "W".ToDirection()
                );
        }

        [TestMethod]
        public void ToCharTest()
        {
            Assert.AreEqual(
                "N",
                Direction.Up.ToChar()
                );

            Assert.AreEqual(
                 "E",
                 Direction.Right.ToChar()
                 );

            Assert.AreEqual(
                "S",
                Direction.Down.ToChar()
                );

            Assert.AreEqual(
                "W",
                Direction.Left.ToChar()
                );
        }

        [TestMethod]
        public void OppositeTest()
        {
            Assert.AreEqual(
                Direction.Down,
                Direction.Up.Opposite()
                );

            Assert.AreEqual(
                Direction.Up,
                Direction.Down.Opposite()
                );

            Assert.AreEqual(
                Direction.Left,
                Direction.Right.Opposite()
                );

            Assert.AreEqual(
                Direction.Right,
                Direction.Left.Opposite()
                );
        }

    }
}
