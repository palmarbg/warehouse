using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DataTypes;
using Persistence.DataTypes;

namespace Test.ModelTests.DataTypeTests
{
    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void SameAsSamePositionTest()
        {
            var node1 = new Node(new Position { X = 1, Y = 2 });
            var node2 = new Node(new Position { X = 1, Y = 2 });
            bool result = node1.SameAs(node2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SameAsDiffPositionTest()
        {
            var node1 = new Node(new Position { X = 1, Y = 2 });
            var node2 = new Node(new Position { X = 3, Y = 4 });

            bool result = node1.SameAs(node2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SameHash()
        {
            var node1 = new Node(new Position { X = 1, Y = 2 });
            var node2 = new Node(new Position { X = 1, Y = 2 });

            int hashCode1 = node1.GetHashCode();
            int hashCode2 = node2.GetHashCode();

            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void ToStringTests()
        {
            var node = new Node(new Position { X = 1, Y = 2 });

            string result = node.ToString();

            Assert.AreEqual("(1,2)", result);
        }

        [TestMethod]
        public void NewWaitNodeTests()
        {
            var node = Node.NewWaitNode();

            Assert.AreEqual(-1, node.Position.X);
            Assert.AreEqual(-1, node.Position.Y);
        }
    }
}
