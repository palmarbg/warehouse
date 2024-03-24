using RobotokModel.Model;
using RobotokModel.Model.Extensions;
using RobotokModel.Persistence.Interfaces;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RobotokModel.Persistence.DataAccesses
{
    public class ConfigDataAccess : IDataAccess
    {
        #region Private fields

        private Uri baseUri;
        private string path;
        private SimulationData simulationData = null!;

        #endregion

        #region Constructor
        public ConfigDataAccess(string path)
        {
            this.path = path;
            baseUri = new(path);
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
            return new ConfigDataAccess(filePath);
        }

        #endregion

        #region Private methods
        private void SetMap(string path)
        {
            string filePath = new Uri(baseUri, path).AbsolutePath;

            string[] mapData = File.ReadAllText(filePath).Split('\n');
            // map[0]: type octile nem tudjuk mit jelent, nem használjuk
            int height = int.Parse(mapData[1].Split(' ')[1]);
            int width = int.Parse(mapData[2].Split(' ')[1]);
            ITile[,] map = new ITile[width, height];
            for (int y = 0; y < height; y++)
            {
                string row = mapData[y+4];
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
        private void SetRobots(string path)
        {
            string filePath = new Uri(baseUri, path).AbsolutePath;

            string[] robotData = File.ReadAllText(filePath).Split('\n');
            int robotCount = int.Parse(robotData[0]);
            for (int i = 1; i <= robotCount; i++)
            {
                int intPos = int.Parse(robotData[i]);
                

                int x = intPos % simulationData.Map.GetLength(0);
                int y = intPos / simulationData.Map.GetLength(0);

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

            string[] goalData = File.ReadAllText(filePath).Split('\n');
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
            try
            {
                baseUri = new(path);
                string jsonString = File.ReadAllText(path);
                var options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;
                options.Converters.Add(new JsonStringEnumConverter());
                Config? config = JsonSerializer.Deserialize<Config>(jsonString, options) ?? throw new JSonError("Serialization of config file was unsuccesful!");
                simulationData = new SimulationData
                {
                    DistributorName = config.TaskAssignmentStrategy,
                    RevealedTaskCount = config.NumTasksReveal,
                    Map = null!,
                    Goals =[],
                    Robots = []
                };
                SetMap(config.MapFile);
                SetRobots(config.AgentFile);
                SetGoals(config.TaskFile);
            }
            catch (Exception) 
            {
                throw new JSonError();
            }
        }
        #endregion

    }
}
