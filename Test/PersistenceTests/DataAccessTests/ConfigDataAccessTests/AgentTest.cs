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
    public class AgentTest
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

                    """
                    5
                    0
                    89
                    4
                    13
                    41
                    86
                    """
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
        public void SetRobotsTests()
        {
            SimulationData data = _configDataAccess.GetInitialSimulationData();

            Assert.IsNotNull(data);
            Assert.AreEqual(data.Robots.Count, 5);

            Assert.IsTrue(data.Map[0, 0] is Robot);
            Assert.IsTrue(data.Map[10, 9] is Robot);
            Assert.IsTrue(data.Map[3, 0] is Robot);
            Assert.IsTrue(data.Map[3, 1] is Robot);
            Assert.IsTrue(data.Map[1, 4] is Robot);

            Assert.IsTrue(data.Map[6, 8] is not Robot);
            Assert.IsTrue(data.Map[0, 3] is not Robot);
            Assert.IsTrue(data.Map[1, 3] is not Robot);
            Assert.IsTrue(data.Map[4, 1] is not Robot);
        }
    }
}
