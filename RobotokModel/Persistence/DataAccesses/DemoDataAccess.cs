using RobotokModel.Model;
using RobotokModel.Model.Extensions;
using RobotokModel.Persistence.Interfaces;

namespace RobotokModel.Persistence.DataAccesses
{
    public class DemoDataAccess : IDataAccess
    {
        public SimulationData SimulationData { get; set; } = null!;

        #region Async
        public async Task LoadAsync(string path)
        {
            await Task.Run(() =>
            {
                SimulationData = new SimulationData
                {
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

        #region Syncronous
        public void Load(string path)
        {
            
            SimulationData = new SimulationData
            {
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

    }
}
