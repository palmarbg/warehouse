namespace TestModel
{
    [TestClass]
    public class TestTest
    {
        [TestMethod]
        public void ConstantTrue()
        {
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void ConstantFalse()
        {
            Assert.IsTrue(false);
        }
    }
}