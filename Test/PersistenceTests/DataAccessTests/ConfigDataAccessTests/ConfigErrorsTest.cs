using Persistence;
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
    public class ConfigErrorsTest
    {
        private DirectoryDataAccess _dirAccess = null!;
        private ConfigDataAccess _configDataAccess = null!;
        private string _files = null!;

        [TestInitialize]
        public void Initialize()
        {
            _dirAccess = new DirectoryDataAccess();
            string path1 = System.IO.Directory.GetCurrentDirectory().Split("/bin")[0] + "/Files/";
            string path2 = System.IO.Directory.GetCurrentDirectory().Split("\\bin")[0] + "/Files/";
            _files = path1.Length < path2.Length ? path1 : path2;
        }
        [TestMethod]
        public void InvalidMapTests()
        {
            _configDataAccess = new ConfigDataAccess(_files + "Config3.json", _dirAccess);
            Assert.ThrowsException<InvalidMapDetailsException>(() => _configDataAccess.GetInitialSimulationData());

            _configDataAccess = new ConfigDataAccess(_files + "Config4.json", _dirAccess);
            Assert.ThrowsException<InvalidMapDetailsException>(() => _configDataAccess.GetInitialSimulationData());

            _configDataAccess = new ConfigDataAccess(_files + "Config5.json", _dirAccess);
            Assert.ThrowsException<InvalidMapDetailsException>(() => _configDataAccess.GetInitialSimulationData());

            _configDataAccess = new ConfigDataAccess(_files + "Config6.json", _dirAccess);
            Assert.ThrowsException<IndexOutOfRangeException>(() => _configDataAccess.GetInitialSimulationData());
        }
        [TestMethod]
        public void InvalidAgentTests()
        {
            _configDataAccess = new ConfigDataAccess(_files + "Config7.json", _dirAccess);
            Assert.ThrowsException<IndexOutOfRangeException>(() => _configDataAccess.GetInitialSimulationData());

            _configDataAccess = new ConfigDataAccess(_files + "Config8.json", _dirAccess);
            Assert.ThrowsException<IndexOutOfRangeException>(() => _configDataAccess.GetInitialSimulationData());

            _configDataAccess = new ConfigDataAccess(_files + "Config9.json", _dirAccess);
            Assert.ThrowsException<IndexOutOfRangeException>(() => _configDataAccess.GetInitialSimulationData());

            _configDataAccess = new ConfigDataAccess(_files + "Config10.json", _dirAccess);
            Assert.ThrowsException<IndexOutOfRangeException>(() => _configDataAccess.GetInitialSimulationData());
        }
    }
}
