using Persistence.DataAccesses;
using Persistence.DataTypes;
using Persistence.Extensions;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.PersistenceTests.DataAccessTests.ConfigDataAccessTests
{
    [TestClass]
    public class AgentTest
    {
        private DirectoryDataAccess _dirAccess = null!;
        private ConfigDataAccess _configDataAccess = null!;
        private string _files = null!;
        SimulationData data = null!;

        [TestInitialize]
        public void Initialize()
        {
            _dirAccess = new DirectoryDataAccess();
            _files = System.IO.Directory.GetCurrentDirectory().Split("\\bin")[0] + "/Files/";
        }
        public void SetRobotsTests()
        {
            _configDataAccess = new ConfigDataAccess(_files + "Config11.json", _dirAccess);
            data = _configDataAccess.GetInitialSimulationData();

            Assert.IsNotNull(data);
            Assert.AreEqual(data.Robots.Count, 20);

            Assert.IsTrue(data.Map[0, 0] is Robot);
            Assert.IsTrue(data.Map[9, 9] is Robot);
            Assert.IsTrue(data.Map[4, 8] is Robot);
            Assert.IsTrue(data.Map[5, 1] is Robot);
            Assert.IsTrue(data.Map[0, 1] is Robot);
            Assert.IsTrue(data.Map[4, 6] is Robot);
            Assert.IsTrue(data.Map[4, 7] is Robot);
            Assert.IsTrue(data.Map[9, 5] is Robot);
            Assert.IsTrue(data.Map[0, 4] is Robot);
            Assert.IsTrue(data.Map[5, 0] is Robot);

            Assert.IsTrue(data.Map[7, 4] is not Robot);
            Assert.IsTrue(data.Map[0, 5] is not Robot);
            Assert.IsTrue(data.Map[3, 1] is not Robot);
            Assert.IsTrue(data.Map[9, 8] is not Robot);
            Assert.IsTrue(data.Map[5, 9] is not Robot);
        }
    }
}
