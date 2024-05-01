using Persistence.DataTypes;
using Persistence.Extensions;
using Persistence.Interfaces;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Persistence.DataAccesses
{
    public class ConfigDataAccess : IDataAccess
    {
        #region Private fields

        private Uri baseUri;
        private string path;
        private SimulationData simulationData = null!;
        private IDirectoryDataAccess _directoryDataAccess;

        #endregion

        #region Constructor
        public ConfigDataAccess(string path, IDirectoryDataAccess directoryDataAccess)
        {
            this.path = path;
            baseUri = new(path);
            _directoryDataAccess = directoryDataAccess;
        }

        #endregion

        #region Public methods
        public SimulationData GetInitialSimulationData()
        {
            Load();
            return simulationData;
        }
        public IDataAccess NewInstance(string filePath)
        {
            return new ConfigDataAccess(filePath, _directoryDataAccess);
        }

        #endregion

        #region Private methods
        private void SetMap(string path)
        {
            string filePath = new Uri(baseUri, path).AbsolutePath;

            string[] mapData = _directoryDataAccess.LoadFromFile(filePath).Split('\n');
            try
            {
                int height = int.Parse(mapData[1].Split(' ')[1]);
                int width = int.Parse(mapData[2].Split(' ')[1]);
                if (width <= 0 || height <= 0)
                    if (width <= 0 || height <= 0)
                    {
                        throw new InvalidMapDetailsException("Height or width is less than 1");
                    }
                ITile[,] map = new ITile[width, height];
                for (int y = 0; y < height; y++)
                {
                    string row = mapData[y + 4];
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
                simulationData.Map = map;
            }
            catch
            {
                throw new InvalidMapDetailsException("Parse error in loading map");
            }

        }
        private void SetRobots(string path)
        {
            string filePath = new Uri(baseUri, path).AbsolutePath;

            string[] robotData = _directoryDataAccess.LoadFromFile(filePath).Split('\n');
            int robotCount = int.Parse(robotData[0]);
            if (robotData.Length - 1 < robotCount)
            {
                throw new InvalidArgumentException("Too few robots in file");
            }
            for (int i = 1; i <= robotCount; i++)
            {
                int intPos = int.Parse(robotData[i]);

                int x = intPos % simulationData.Map.GetLength(0);
                int y = intPos / simulationData.Map.GetLength(0);
                if (simulationData.Map[x, y] is not EmptyTile)
                {
                    throw new InvalidRobotPositionException("Trying to place a robot to a non-empty tile while loading config");
                }

                Robot r = new Robot
                {
                    Id = i - 1,
                    Position = new Position { X = x, Y = y },
                    Rotation = Direction.Right
                };
                simulationData.Robots.Add(r);
                simulationData.Map.SetAtPosition(r.Position, r);
            }
        }
        private void SetGoals(string path)
        {
            string filePath = new Uri(baseUri, path).AbsolutePath;

            string[] goalData = _directoryDataAccess.LoadFromFile(filePath).Split('\n');
            int goalCount = int.Parse(goalData[0]);
            for (int i = 1; i <= goalCount; i++)
            {
                int intPos = int.Parse(goalData[i]);
                int x = intPos % simulationData.Map.GetLength(0);
                int y = intPos / simulationData.Map.GetLength(0);

                Goal g = new Goal
                {
                    Id = i - 1,
                    Position = new Position { X = x, Y = y },
                };
                simulationData.Goals.Add(g);
            }
        }
        private void Load()
        {
            baseUri = new(path);
            string jsonString = _directoryDataAccess.LoadFromFile(path);
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new JsonStringEnumConverter());
            Config? config = JsonSerializer.Deserialize<Config>(jsonString, options) ?? throw new JSonError("Serialization of config file was unsuccesful!");
            simulationData = new SimulationData
            {
                DistributorName = config.TaskAssignmentStrategy,
                RevealedTaskCount = config.NumTasksReveal,
                Map = null!,
                Goals = [],
                Robots = []
            };
            SetMap(config.MapFile);
            SetRobots(config.AgentFile);
            SetGoals(config.TaskFile);
        }
        #endregion

    }
}
