using Persistence.DataTypes;
using Persistence.Extensions;

namespace TestModel.PersistenceTests.ExtensionTests
{
    [TestClass]
    public class PositionExtensionTests
    {
        Position newPosition14;
        Position newPosition24;
        Position newPosition15;
        Position newPosition25;
        public PositionExtensionTests() 
        {
            newPosition14.X = 1;
            newPosition14.Y = 4;

            newPosition24.X = 2;
            newPosition24.Y = 4;

            newPosition15.X = 1;
            newPosition15.Y = 5;

            newPosition25.X = 2;
            newPosition25.Y = 5;
        }

        [TestMethod]
        public void PositionInDirectionTests()
        {
            Assert.AreEqual(
                newPosition15,
                newPosition14.PositionInDirection(Direction.Down)
                );
            Assert.AreEqual(
                newPosition24,
                newPosition14.PositionInDirection(Direction.Right)
                );
            Assert.AreEqual(
                newPosition24,
                newPosition25.PositionInDirection(Direction.Up)
                );
            Assert.AreEqual(
                newPosition14,
                newPosition24.PositionInDirection(Direction.Left)
                );
        }









    }
}
