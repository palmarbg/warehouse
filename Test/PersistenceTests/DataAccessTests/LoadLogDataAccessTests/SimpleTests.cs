using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Persistence.DataAccesses;
using Persistence.DataTypes;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Test.PersistenceTests.DataAccessTests.LoadLogDataAccessTests
{
    [TestClass]
    public class LoadLogDataAccessTests
    {
        private LoadLogDataAccess _loadLogDataAccess;
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
            _dirAccess = new();
            _configDataAccess = new ConfigDataAccess(_files + "random_20_config.json" ,_dirAccess);
            _loadLogDataAccess = new LoadLogDataAccess(_files + "log1.json", _configDataAccess);
        }


        [TestMethod]
        public void GetDataTest()
        {
            SimulationData expexted = _configDataAccess.GetInitialSimulationData();
            SimulationData res = _loadLogDataAccess.GetInitialSimulationData();
            Assert.IsNotNull(expexted);
            Assert.IsNotNull(res);
            Assert.AreEqual(expexted.Map.GetLength(0),res.Map.GetLength(0));
            Assert.AreEqual(expexted.Map.GetLength(1), res.Map.GetLength(1));
            Assert.AreEqual(expexted.RevealedTaskCount,res.RevealedTaskCount);
            Assert.AreEqual(0 ,res.Goals[0].Id);
            Assert.AreEqual(new() { X = 6, Y = 21 }, res.Goals[0].Position);
            Assert.IsFalse(res.Goals[0].IsAssigned);
            Assert.AreEqual(expexted.DistributorName, res.DistributorName);

            Assert.AreEqual(149, res.Goals[149].Id);
            Assert.AreEqual(new() { X = 1, Y = 2 }, res.Goals[149].Position);
            Assert.IsFalse(res.Goals[149].IsAssigned);
            Assert.AreEqual(expexted.DistributorName, res.DistributorName);

        }
        [TestMethod]
        public void LoadLogTest()
        {
            var log = _loadLogDataAccess.LoadLog(_files + "log1.json");
            Assert.IsNotNull(log);
        }

    }
}
