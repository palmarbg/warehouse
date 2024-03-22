using RobotokModel.Model;
using RobotokModel.Model.Extensions;
using RobotokModel.Persistence.Interfaces;
using System.Text.Json;

namespace RobotokModel.Persistence.DataAccesses
{
    public class ConfigDataAccess : IDataAccess
    {
        #region Private

        private Uri baseUri;

        public ConfigDataAccess()
        {
            this.baseUri = new("C:\\");
        }

        #endregion
        public SimulationData SimulationData { get; set; } = null!;

        #region Async
        public async Task LoadAsync(string path)
        {
            try
            {
                //TODO
            }
            catch
            {
                throw new JSonError();
            }

            await Task.Run(() =>
            {
                SimulationData = new SimulationData
                {
                    DistributionStrategy = Strategy.RoundRobin,
                    RevealedTaskCount = 1,
                    Map = new ITile[10, 10],
                    Goals =
                    [
                        new Goal
                        {
                            Position = new Position {X = 0,Y = 0},
                            Id = 0
                        },
                        new Goal
                        {
                            Position = new Position {X = 2,Y = 2},
                            Id = 1
                        },
                        new Goal
                        {
                            Position = new Position {X = 3,Y = 3},
                            Id = 2
                        }
                    ],
                    Robots =
                    [
                        new Robot
                        {
                            Id = 0,
                            Position = new Position { X = 0,Y = 4},
                            Rotation = Direction.Right
                        },
                        new Robot
                        {
                            Id = 1,
                            Position = new Position { X = 1,Y = 4},
                            Rotation = Direction.Right
                        },
                        new Robot
                        {
                            Id = 2,
                            Position = new Position { X = 2,Y = 4},
                            Rotation = Direction.Right
                        }
                    ]
                };
            });

            for (int i = 0; i < SimulationData.Map.GetLength(0); i++)
            {
                for (int j = 0; j < SimulationData.Map.GetLength(1); j++)
                {
                    SimulationData.Map[i, j] = EmptyTile.Instance;
                }
            }

            foreach (Robot robot in SimulationData.Robots)
            {
                SimulationData.Map.SetAtPosition(robot.Position, robot);
            }
        }
        #endregion

        #region SideMethods
        private void SetMap(string path)
        {
            string filePath = new Uri(baseUri, path).ToString();

            string[] mapData = File.ReadAllText(filePath).Split('\n');
            // map[0]: type octile nem tudjuk mit jelent, nem használjuk
            int height = int.Parse(mapData[1].Split(' ')[1]);
            int width = int.Parse(mapData[2].Split(' ')[1]);
            ITile[,] map = new ITile[width, height];
            for (int i = 3; i < mapData.Length; i++)
            {
                int y = i - 3;
                string row = mapData[y];
                for (int x = 0; x < width; x++)
                {
                    if (row[x] == '.')
                    {
                        map[x, y] = EmptyTile.Instance;
                    }
                    else
                    {
                        map[x, y] = Block.Instance;
                    }
                }
            }
            SimulationData.Map = map;
        }
        private void SetRobots(string path)
        {
            string filePath = new Uri(baseUri, path).ToString();

            string[] robotData = File.ReadAllText(filePath).Split('\n');
            int robotCount = int.Parse(robotData[0]);
            for (int i = 1; i <= robotCount; i++)
            {
                int intPos = int.Parse(robotData[i]);
                

                int x = intPos / SimulationData.Map.GetLength(0);
                int y = intPos % SimulationData.Map.GetLength(0);
                if (x > 0) { x--; }
                if (y > 0) { y--; }

                Robot r = new Robot
                {
                    Id = i - 1,
                    Position = new Position { X = x, Y = y },
                    Rotation = Direction.Right
                };
                SimulationData.Robots.Add(r);
            }
        }
        private void SetGoals(string path)
        {
            string filePath = new Uri(baseUri, path).ToString();

            string[] goalData = File.ReadAllText(filePath).Split('\n');
            int goalCount = int.Parse(goalData[0]);
            for (int i = 1; i <= goalCount; i++)
            {
                int intPos = int.Parse(goalData[i]);
                int x = intPos / SimulationData.Map.GetLength(0);
                int y = intPos % SimulationData.Map.GetLength(0);
                if (x > 0) { x--; }
                if (y > 0) { y--; }

                Goal g = new Goal
                {
                    Id = i - 1,
                    Position = new Position { X = x, Y = y },
                };
                SimulationData.Goals.Add(g);
            }
        }
        #endregion

        #region Syncronous
        public void Load(string path)
        {
            try
            {
                baseUri = new(Path.GetDirectoryName(path));

                string jsonString = File.ReadAllText(path);
                Config? config = JsonSerializer.Deserialize<Config>(jsonString) ?? throw new JSonError("Serialization of config file was unsuccesful!");
                Strategy strategy;
                switch (config.taskAssignmentStrategy)
                {
                    case "roundrobin":
                        strategy = Strategy.RoundRobin;
                        break;
                    case "a*":
                        strategy = Strategy.AStar;
                        break;
                    default:
                        strategy = Strategy.RoundRobin;
                        break;
                }
                SimulationData = new SimulationData
                {
                    DistributionStrategy = strategy,
                    RevealedTaskCount = config.numTasksReveal,
                    Map = null!,
                    Goals =[],
                    Robots = []
                };
                SetMap(config.mapFile);
                SetRobots(config.agentFile);
                SetGoals(config.taskFile);
            }
            catch
            {
                throw new JSonError();
            }
        }
        #endregion

    }
}
