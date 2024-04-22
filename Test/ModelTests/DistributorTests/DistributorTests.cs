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
    class DistributorTests
    {
        private SimulationData _simulationData = null!;
        private DemoDistributor distributor = null!;
        private Mock<ConfigDataAccess> _mock = null!;
        [TestMethod]
        public void TestMoq()
        {
            _mock = new Mock<ConfigDataAccess>();
        }

    }
}
