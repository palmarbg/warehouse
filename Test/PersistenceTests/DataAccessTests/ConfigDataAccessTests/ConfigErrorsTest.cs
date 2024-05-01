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
using Test.MockClasses.DataAccesses;

namespace Test.PersistenceTests.DataAccessTests.ConfigDataAccessTests
{
    [TestClass]
    public class ConfigErrorsTest
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
        public void InvalidMapSize()
        {
            Dictionary<string, string> mockDirectory = new()
            {
                {
                    "X:/map.map",

                    "type octile\n" +
                    "height 13\n" +
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
            Assert.ThrowsException<InvalidMapDetailsException>(() => _configDataAccess.GetInitialSimulationData());

            mockDirectory = new()
            {
                {
                    "X:/map.map",

                    "type octile\n" +
                    "height 9\n" +
                    "width -1\n" +
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
            Assert.ThrowsException<InvalidMapDetailsException>(() => _configDataAccess.GetInitialSimulationData());

            mockDirectory = new()
            {
                {
                    "X:/map.map",

                    "type octile\n" +
                    "height 0\n" +
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
            Assert.ThrowsException<InvalidMapDetailsException>(() => _configDataAccess.GetInitialSimulationData());
        }
        [TestMethod]
        public void InvalidAgents()
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
                    1
                    2
                    3
                    4
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
            Assert.ThrowsException<InvalidArgumentException>(() => _configDataAccess.GetInitialSimulationData());

            mockDirectory = new()
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
                    1
                    2
                    3
                    4
                    4
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
            Assert.ThrowsException<InvalidRobotPositionException>(() => _configDataAccess.GetInitialSimulationData());

            mockDirectory = new()
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
                    1
                    90
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
            Assert.ThrowsException<IndexOutOfRangeException>(() => _configDataAccess.GetInitialSimulationData());

            mockDirectory = new()
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
                    1
                    -1
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
            Assert.ThrowsException<IndexOutOfRangeException>(() => _configDataAccess.GetInitialSimulationData());

        }
    }
}
