using RobotokModel.Model;
using RobotokModel.Model.Extensions;
using RobotokModel.Persistence.Interfaces;

namespace RobotokModel.Persistence.DataAccesses
{
    public class DemoDataAccess : IDataAccess
    {
        #region Private fields

        private string path;
        private SimulationData simulationData = null!;

        #endregion

        #region Constructor
        public DemoDataAccess(string path)
        {
            this.path = path;
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
            return new DemoDataAccess(filePath);
        }

        #endregion

        #region Private methods

        private void Load()
        {
            simulationData = new SimulationData
            {
                DistributorName = "roundrobin",
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
                        Position = new Position {X = 1,Y = 2},
                        Id = 1
                    },
                    new Goal
                    {
                        Position = new Position {X = 2,Y = 3},
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

            for (int i = 0; i < simulationData.Map.GetLength(0); i++)
            {
                for (int j = 0; j < simulationData.Map.GetLength(1); j++)
                {
                    simulationData.Map[i, j] = EmptyTile.Instance;
                }
            }

            simulationData.Map[2, 5] = Block.Instance;

            foreach (Robot robot in simulationData.Robots)
            {
                simulationData.Map.SetAtPosition(robot.Position, robot);
            }
        }

        #endregion
    }
}
