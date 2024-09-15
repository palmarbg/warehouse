using Persistence.Extensions;

namespace TestModel.PersistenceTests.ExtensionTests
{
    [TestClass]
    public class MapExtensionTests
    {
        private int[,] _small = null!;
        private bool[,] _medium = null!;
        private int[,] _large = null!;

        [TestInitialize]
        public void Initialize()
        {
            _small = new int[3, 2];
            _medium = new bool[15, 27];
            _large = new int[200, 700];
        }

        [TestMethod]
        public void GetWidthTest()
        {
            Assert.AreEqual(3, _small.GetWidth());

            Assert.AreEqual(15, _medium.GetWidth());

            Assert.AreEqual(200, _large.GetWidth());

        }

        [TestMethod]
        public void GetHeightTest()
        {
            Assert.AreEqual(2, _small.GetHeight());

            Assert.AreEqual(27, _medium.GetHeight());

            Assert.AreEqual(700, _large.GetHeight());

        }

        [TestMethod]
        public void IntToXYTest()
        {
            int count = 0;

            for (int y = 0; y < _small.GetHeight(); y++)
                for (int x = 0; x < _small.GetWidth(); x++)
                {
                    Assert.AreEqual((x, y), count.ToXY(_small));
                    count++;
                }

            count = 0;

            for (int y = 0; y < _medium.GetHeight(); y++)
                for (int x = 0; x < _medium.GetWidth(); x++)
                {
                    Assert.AreEqual((x, y), count.ToXY(_medium));
                    count++;
                }

            //test last element
            Assert.AreEqual(
                (_large.GetWidth() - 1, _large.GetHeight() - 1),
                (_large.GetWidth() * _large.GetHeight() - 1).ToXY(_large)
                );

        }
    }
}
