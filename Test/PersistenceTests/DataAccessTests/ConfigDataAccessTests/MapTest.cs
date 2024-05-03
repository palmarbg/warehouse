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
    public class MapTest
    {
        private DirectoryDataAccess _dirAccess = null!;
        private ConfigDataAccess _configDataAccess = null!;
        private string _files = null!;
        SimulationData data = null!;

        [TestInitialize]
        public void Initialize()
        {
            _dirAccess = new DirectoryDataAccess();
            string path1 = System.IO.Directory.GetCurrentDirectory().Split("/bin")[0] + "/Files/";
            string path2 = System.IO.Directory.GetCurrentDirectory().Split("\\bin")[0] + "/Files/";
            _files = path1.Length < path2.Length ? path1 : path2;
        }
        [TestMethod]
        public void MapTests()
        {
            _configDataAccess = new ConfigDataAccess(_files + "Config2.json", _dirAccess);
            data = _configDataAccess.GetInitialSimulationData();
            Assert.IsNotNull(data);
            Assert.AreEqual(data.Map.GetLength(0), 5);
            Assert.AreEqual(data.Map.GetLength(1), 5);
            /*
            a.c.e
            \@&#-
            ß.$.í
            ..5..
            .é.p.
            */
            Assert.IsTrue(data.Map[0, 0] is Block);
            Assert.IsTrue(data.Map[1, 0] is not Block);
            Assert.IsTrue(data.Map[2, 0] is Block);
            Assert.IsTrue(data.Map[3, 0] is not Block);
            Assert.IsTrue(data.Map[4, 0] is Block);

            Assert.IsTrue(data.Map[0, 1] is Block);
            Assert.IsTrue(data.Map[1, 1] is Block);
            Assert.IsTrue(data.Map[2, 1] is Block);
            Assert.IsTrue(data.Map[3, 1] is Block);
            Assert.IsTrue(data.Map[4, 1] is Block);

            Assert.IsTrue(data.Map[0, 2] is Block);
            Assert.IsTrue(data.Map[1, 2] is not Block);
            Assert.IsTrue(data.Map[2, 2] is Block);
            Assert.IsTrue(data.Map[3, 2] is not Block);
            Assert.IsTrue(data.Map[4, 2] is Block);

            Assert.IsTrue(data.Map[0, 3] is not Block);
            Assert.IsTrue(data.Map[1, 3] is not Block);
            Assert.IsTrue(data.Map[2, 3] is Block);
            Assert.IsTrue(data.Map[3, 3] is not Block);
            Assert.IsTrue(data.Map[4, 3] is not Block);

            Assert.IsTrue(data.Map[0, 4] is not Block);
            Assert.IsTrue(data.Map[1, 4] is Block);
            Assert.IsTrue(data.Map[2, 4] is not Block);
            Assert.IsTrue(data.Map[3, 4] is Block);
            Assert.IsTrue(data.Map[4, 4] is not Block);

        }
    }
}
