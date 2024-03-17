using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotokModel.Model.Interfaces;

namespace RobotokModel.Model
{
    /// <summary>
    /// Dijsktra algorithm for shortest path
    /// considers robots as stationary Blocks
    /// </summary>
    internal class PrimitiveController : IController
    {
        int[] dRow = { -1, 0, 1, 0 };
        int[] dCol = { 0, 1, 0, -1 };
        bool[,] visited;
        Position?[,] parents;
        int[,] distances;

        public SimulationData SimulationData { get; set; }

        public PrimitiveController()
        {
            
            this.visited = GenerateBoolMap();
            this.parents = new Position?[SimulationData.Map.GetLength(0), SimulationData.Map.GetLength(1)];
            this.distances = new int[SimulationData.Map.GetLength(0), SimulationData.Map.GetLength(1)];
        }
        private void InitMatrixes()
        {
            this.visited = GenerateBoolMap();
            this.parents = new Position?[SimulationData.Map.GetLength(0), SimulationData.Map.GetLength(1)];
            this.distances = new int[SimulationData.Map.GetLength(0), SimulationData.Map.GetLength(1)];
            for (int i = 0; i < distances.GetLength(0); i++)
            {
                for (int j = 0; j < distances.GetLength(1); j++)
                {
                    distances[i, j] = int.MaxValue;
                }
            }

        }
        public RobotOperation[] ClaculateOperations()
        {
            throw new NotImplementedException();
        }

        public RobotOperation[] NextStep()
        {
            return Robot.Robots.Select(NextOperation).ToArray();
        }
        private RobotOperation NextOperation(Robot robot)
        {
            var nextPosition = NextPosition(robot);
            throw new NotImplementedException();

        }

        private Position NextPosition(Robot robot)
        {
            InitMatrixes();
            // may cause problems 
            parents.SetAtPosition(robot.Position, robot.Position);
            distances.SetAtPosition(robot.Position, 0);
            var minPositionQueue = new PriorityQueue<Position, int>(Comparer<int>.Create((a,b) => b-a));
            //fill queue with vertices and distances
            for (int i = 0; i < distances.GetLength(0); i++)
            {
                for (int j = 0; j < distances.GetLength(1); j++)
                {
                    var pos = new Position() { X = i, Y = j };
                    minPositionQueue.Enqueue(pos,distances.GetAtPosition(pos));
                }
            }
            var u = robot.Position;
            while (distances.GetAtPosition(u) < int.MaxValue && minPositionQueue.Count > 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    var v = new Position();
                    v.X = u.X + dRow[i];
                    v.Y = u.Y + dCol[i];
                    if(distances.GetAtPosition(v) > distances.GetAtPosition(u) + 1)
                    {
                        distances.SetAtPosition(u, distances.GetAtPosition(u)+1);
                        parents.SetAtPosition(v,u);
                        // change priority
                    }

                }
                u = minPositionQueue.Dequeue();
            }


            throw new NotImplementedException();

        }
        private bool PositionValid(Position position)
        {
            if (position.X < 0 || position.Y < 0 ||
                position.X >= SimulationData.Map.GetLength(0) ||
                position.Y >= SimulationData.Map.GetLength(1))
            {
                return false;
            }
            else if (!SimulationData.Map.GetAtPosition(position).IsPassable)
            {
                return false;
            }
            else if (visited.GetAtPosition(position))
            {
                return false;
            }
            return true;
        }
        private bool[,] GenerateBoolMap() => new bool[SimulationData.Map.GetLength(0), SimulationData.Map.GetLength(1)];

    }
}
