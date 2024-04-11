using RobotokModel.Model.Extensions;
using RobotokModel.Model.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RobotokModel.Model.Controllers
{
    internal class AStarController : IController
    {
        public string Name => "AStarController";
        private SimulationData? SimulationData = null!;
        private ITaskDistributor _taskDistributor = null!;
        private List<Queue<RobotOperation>> _plannedOperations = new List<Queue<RobotOperation>>();

        public event EventHandler<IControllerEventArgs> FinishedTask;

        public class Node
        {
            public Position Position { get; set; }
            public int gCost;
            public int hCost;
            public int fCost => gCost + hCost;
            public Node? parent;
            public Node(Position pos)
            {
                this.Position = pos;
            }
            public bool SameAs(Node other)
            {
                return this.Position.Equals(other.Position);
            }
            public override bool Equals(object? obj)
            {
                if (obj is Node node)
                    return this.Position.EqualsPosition(node.Position);
                else
                    return base.Equals(obj);
            }
            // https://stackoverflow.com/questions/371328/why-is-it-important-to-override-gethashcode-when-equals-method-is-overridden
            public override int GetHashCode()
            {
                unchecked // overflow 
                {
                    int hash = 13;
                    hash = hash * 3 + Position.X.GetHashCode();
                    hash = hash * 7 + Position.Y.GetHashCode();
                    return hash;
                }
            }

            public override string ToString()
            {
                return Position.ToString();
            }
        }
        // TODO check nulls
        public void CalculateOperations(TimeSpan timeSpan)
        {
            var result = _plannedOperations.Select(f => f.Dequeue()).ToArray();
            for (int i = 0; i < SimulationData.Robots.Count; i++)
            {
                SimulationData.Robots[i].NextOperation = result[i];
            }
            OnTaskFinished(result.ToArray());
        }
        private void OnTaskFinished(RobotOperation[] result)
        {
            FinishedTask?.Invoke(this, new(result));
        }
        private Queue<RobotOperation> FindPath(Robot robot)
        {
            HashSet<Node> openSet = new HashSet<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
            Dictionary<Node, int> gScore = new Dictionary<Node, int>();
            Dictionary<Node, int> fScore = new Dictionary<Node, int>();
            var start = new Node(robot.Position);
            var goal = new Node(robot.CurrentGoal.Position);
            openSet.Add(start);

            gScore[start] = 0;
            fScore[start] = Heuristic(start, goal);

            while (openSet.Count > 0)
            {
                Node current = null;
                int lowestFScore = int.MaxValue;
                foreach (var node in openSet)
                {
                    if (fScore.ContainsKey(node) && fScore[node] < lowestFScore)
                    {
                        lowestFScore = fScore[node];
                        current = node;
                    }
                }

                if (current.Position.EqualsPosition(goal.Position))
                {
                    return RetracePath(start, current);
                }

                closedSet.Add(current);
                openSet.Remove(current);

                foreach (var neighbor in GetNeighbors(current))
                {
                    Direction asd = current.Position.DirectionInPosition(neighbor.Position) ?? Direction.Down;
                    int tentativeGScore = gScore[current] + EuclideanDistance(current, neighbor);
                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        neighbor.parent = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goal);
                        if ( !openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            return null; // No path found
        }

        private int Heuristic(Node a, Node b)
        {
            return Math.Abs(a.Position.X - b.Position.X) + Math.Abs(a.Position.X - b.Position.X);
        }

        private int EuclideanDistance(Node a, Node b)
        {
            return (int)Math.Sqrt(Math.Pow(a.Position.X - b.Position.X, 2) + Math.Pow(a.Position.X - b.Position.X, 2));
        }


        private Queue<RobotOperation> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Add(startNode);

            path.Reverse();
            return NodePathToRobotOperationList(path);
        }
        private Queue<RobotOperation> NodePathToRobotOperationList(List<Node> path)
        {
            Queue<RobotOperation > Operations = new Queue<RobotOperation>();
            var robot = SimulationData.Map.GetAtPosition(path[0].Position);
            Direction currentDirection = Direction.Down;
            if ( robot is not Robot)
            {
                throw new Exception("Path does not start at a robot");
            }
            else
            {
                currentDirection = ((Robot) robot).Rotation;
            }

            for (int i = 1; i < path.Count; i++) {
                var startPos = path[i-1].Position;
                var endPos = path[i].Position;
                var dir = startPos.DirectionInPosition(endPos);
                if (dir == null)
                {
                    continue;
                }
                else
                {
                    var ops = currentDirection.RotateTo((Direction)dir);
                    for (int j = 0;j < ops.Count; j++)
                    {
                        Operations.Enqueue(ops[j]);
                    }
                    Operations.Enqueue(RobotOperation.Forward);
                }
            }
            return Operations;
        }
        private List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();

            for (int xOffset = -1; xOffset <= 1; xOffset++)
            {
                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    if (xOffset != 0 && yOffset != 0 || (xOffset == 0 && yOffset == 0))
                        continue;

                    int checkX = node.Position.X + xOffset;
                    int checkY = node.Position.Y + yOffset;

                    if (checkX >= 0 && checkX < SimulationData!.Map.GetLength(0) && checkY >= 0 && checkY < SimulationData!.Map.GetLength(1))
                    {
                        neighbors.Add(new Node(new Position() { X = checkX, Y = checkY }));
                    }
                }
            }

            return neighbors;
        }

        private bool NeighborIsPassable(Node node)
        {
            return SimulationData!.Map.GetAtPosition(node.Position).IsPassable;
        }

        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan, ITaskDistributor distributor)
        {
            _taskDistributor = distributor;
            this.SimulationData = simulationData;
            simulationData.Robots.ForEach(robot => { if (robot.CurrentGoal is null) _taskDistributor.AssignNewTask(robot);  });
            Goal.OnGoalsChanged();
            _plannedOperations = SimulationData.Robots.Select(FindPath).ToList();
            var asd = 0;
        }

        public IController NewInstance()
        {
            return new AStarController();
        }
    }
}
