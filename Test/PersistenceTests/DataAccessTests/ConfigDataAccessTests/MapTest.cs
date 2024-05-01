using Persistence.DataAccesses;
using Persistence.DataTypes;
using Persistence.Extensions;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.MockClasses.DataAccesses;

namespace Test.PersistenceTests.DataAccessTests.ConfigDataAccessTests
{
    [TestClass]
    public class MapTest
    {
        private MockDirectoryLoadDataAccess _dirAccess = null!;
        private ConfigDataAccess _configDataAccess = null!;

        [TestInitialize]
        public void Initialize()
        {
            Dictionary<string, string> mockDirectory = new()
            {
                {
                    "X:/map.map",

                    "type octile\n" +
                    "height 9\n" +
                    "width 10\n" +
                    "map\n" +
                    "..........\n" +
                    "@@........\n" +
                    "..@.......\n" +
                    "..@.......\n" +
                    "..@@@.....\n" +
                    ".....u....\n" +
                    "......t...\n" +
                    "..b.......\n" +
                    "..........\n"
                },
                {
                    "X:/agents.agents",

                    "0"
                },
                {
                    "X:/tasks.tasks",

                    "0"
                },
                {
                    "X:/config.json",

                    "{\n    " +
                    "\"mapFile\": \"map.map\",\n    " +
                    "\"agentFile\": \"agents.agents\",\n    " +
                    "\"teamSize\": 0,\n    " +
                    "\"taskFile\": \"tasks.tasks\",\n    " +
                    "\"numTasksReveal\": 1,\n    " +
                    "\"taskAssignmentStrategy\": \"roundrobin\"\n  " +
                    "}"
                }
            };
            _dirAccess = new MockDirectoryLoadDataAccess(mockDirectory);
            _configDataAccess = new("X:/config.json", _dirAccess);
        }
        [TestMethod]
        public void MapTests()
        {
            SimulationData data = _configDataAccess.GetInitialSimulationData();
            Assert.IsNotNull(data);
            Assert.AreEqual(data.Map.GetLength(0), 10);
            Assert.AreEqual(data.Map.GetLength(1), 9);
            Assert.IsTrue(data.Map[5, 5] is Block);
            Assert.IsTrue(data.Map[6, 6] is Block);
            Assert.IsTrue(data.Map[0, 1] is Block);
            Assert.IsTrue(data.Map[2, 8] is not Block);
            Assert.IsTrue(data.Map[0, 0] is not Block);
            Assert.IsTrue(data.Map[3, 8] is not Block);
            Assert.IsTrue(data.Map[3, 7] is not Block);
            Assert.IsTrue(data.Map[9, 8] is not Block);
        }
    }
}
