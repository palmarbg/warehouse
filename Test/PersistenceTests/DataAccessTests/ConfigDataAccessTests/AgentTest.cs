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
        }
    }
}
