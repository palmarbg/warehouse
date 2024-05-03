using Persistence.DataAccesses;
using Persistence.DataTypes;
using Persistence.Extensions;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.PersistenceTests.DataAccessTests.SaveLogDataAccessTests
{
    [TestClass]
    public class SimpleTest
    {
        private DirectoryDataAccess _dirAccess = null!;
        private ConfigDataAccess _configDataAccess = null!;
        private string _files = null!;
        SimulationData data = null!;
        private SaveLogDataAccess _saveLogDataAccess = null!;

        [TestInitialize]
        public void Initialize()
        {
            _dirAccess = new DirectoryDataAccess();
            _saveLogDataAccess = new SaveLogDataAccess();
            _files = System.IO.Directory.GetCurrentDirectory().Split("\\bin")[0] + "/Files/";
        }
        [TestMethod]
        public void SimpleTests()
        {
            _configDataAccess = new ConfigDataAccess(_files + "Config1.json", _dirAccess);
            data = _configDataAccess.GetInitialSimulationData();
            Assert.IsNotNull(data);
            Assert.AreEqual(data.Map.GetLength(0), 10);
            Assert.AreEqual(data.Map.GetLength(1), 10);
            Assert.AreEqual(data.Robots.Count, 0);
            Assert.AreEqual(data.DistributorName, "roundrobin");
            Assert.AreEqual(data.RevealedTaskCount, 1);
            Assert.AreEqual(data.Goals.Count, 0);

            _configDataAccess = new ConfigDataAccess(_files + "Config1.json", _dirAccess);
            data = _configDataAccess.GetInitialSimulationData();
        }
        [TestMethod]
        public void ExternalLogTests()
        {
            Log log = new();
            _saveLogDataAccess.GetExternalLog(log);
        }
    }
}
