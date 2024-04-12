using RobotokModel.Model.Extensions;
using RobotokModel.Model.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        // true if robot finished its task and no more tasks are available, false otherwise
        private bool[] robotsFinished = [];
        private RobotOperation[] previousOperations = [];
        private int[] blockedCount = [];
        public event EventHandler<IControllerEventArgs>? FinishedTask;

        #region Public Methods
        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan, ITaskDistributor distributor)
        {
            _taskDistributor = distributor;
            this.SimulationData = simulationData;
            robotsFinished = new bool[simulationData.Robots.Count];
            previousOperations = new RobotOperation[simulationData.Robots.Count];
            blockedCount = new int[simulationData.Robots.Count];
            for (int i = 0; i < simulationData.Robots.Count; i++)
            {
                if (simulationData.Robots[i].CurrentGoal is null) _taskDistributor.AssignNewTask(simulationData.Robots[i]);
                if (simulationData.Robots[i].CurrentGoal is null) robotsFinished[i] = true;
                else robotsFinished[i] = false;
            }
            simulationData.Robots.ForEach(robot =>
            {
                if (robot.CurrentGoal is null) _taskDistributor.AssignNewTask(robot);

            });
            Goal.OnGoalsChanged();
            _plannedOperations = SimulationData.Robots.Select(f=>FindPath(f)).ToList();
        }
        public IController NewInstance()
        {
            return new AStarController();
        }

        // TODO: Final Prototype : 
        public void CalculateOperations(TimeSpan timeSpan)
        {
            if (SimulationData is null) { throw new Exception("initialize the controller first"); }
            var result = new List<RobotOperation>();
            for (int i = 0; i < _plannedOperations.Count; i++)
            {
                var operationQueue = _plannedOperations[i];
                var robot = SimulationData.Robots[i];
                if (operationQueue.Count == 0)
                {
                    if (robotsFinished[i])
                    {
                        SimulationData.Robots[i].NextOperation = RobotOperation.Wait;
                        previousOperations[i] = RobotOperation.Wait;
                        result.Add(RobotOperation.Wait);
                        continue;
                    }
                    if (robot.CurrentGoal is null)
                    {
                        if (_taskDistributor.AllTasksAssigned)
                        {
                            robotsFinished[i] = true;
                            SimulationData.Robots[i].NextOperation = RobotOperation.Wait;
                            previousOperations[i] = RobotOperation.Wait;
                            result.Add(RobotOperation.Wait);
                            continue;
                        }
                        else
                        {
                            _taskDistributor.AssignNewTask(SimulationData.Robots[i]);
                            Goal.OnGoalsChanged();

                            _plannedOperations[i] = FindPath(SimulationData.Robots[i]);
                            var nextOp = NextOperation(i, robot);
                            blockedCount[i] = 0;
                            SimulationData.Robots[i].NextOperation = nextOp;
                            result.Add(nextOp);
                        }
                    }
                }
                else
                {
                    var nextOp = NextOperation(i, robot);

                    SimulationData.Robots[i].NextOperation = nextOp;
                    result.Add(nextOp);
                }
            }
            OnTaskFinished([.. result]);
        }
        // Deadlock
        private RobotOperation NextOperation(int i, Robot robot)
        {
            RobotOperation nextOp;
            //if(i == 3) { Debug.WriteLine(blockedCount[i]); Debug.WriteLine(robot.CurrentGoal.Position.ToString()); }
            if (_plannedOperations[i].Count == 0)
            {
                robotsFinished[i] = true;
                if (robot.CurrentGoal is not null)
                {
                    robot.CurrentGoal.IsAssigned = false;
                    robot.CurrentGoal = null;
                }
                return RobotOperation.Wait;
            }
            if (robot.BlockedThisTurn)
            {
                if (blockedCount[i] >= 3)
                {
                    blockedCount[i]=0;
                    _plannedOperations[i] = FindPath(robot,true);
                    previousOperations[i] = RobotOperation.Wait;
                    return RobotOperation.Wait;
                }
                nextOp = previousOperations[i];
                blockedCount[i]++;
            }
            else
            {
                blockedCount[i] = 0;
                nextOp = _plannedOperations[i].Dequeue();
            }
            previousOperations[i] = nextOp;
            return nextOp;
        }
        #endregion
        #region Private Methods
        private Queue<RobotOperation> FindPath(Robot robot,bool robotBlock = false)
        {
            if (robot.CurrentGoal is null) { throw new Exception("Robot does not have a goal"); }
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
                Node current = null!;
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

                foreach (var neighbor in GetNeighbors(current, robotBlock))
                {
                    Direction asd = current.Position.DirectionInPosition(neighbor.Position) ?? Direction.Down;
                    int tentativeGScore = gScore[current] + EuclideanDistance(current, neighbor);
                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        neighbor.parent = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goal);
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            return new Queue<RobotOperation>(); // No path found
        }

        private Queue<RobotOperation> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;
            if (currentNode is null || (currentNode.parent is null && currentNode != startNode)) { throw new Exception("Unexpected error in Pathfinding"); }

            while (currentNode != startNode)
            {
                path.Add(currentNode!);
                currentNode = currentNode.parent!;
            }
            path.Add(startNode);

            path.Reverse();
            return NodePathToRobotOperationList(path);
        }

        private Queue<RobotOperation> NodePathToRobotOperationList(List<Node> path)
        {
            if (SimulationData is null) { throw new Exception("initialize the controller first"); }
            Queue<RobotOperation> Operations = new Queue<RobotOperation>();
            var robot = SimulationData.Map.GetAtPosition(path[0].Position);
            Direction currentDirection = Direction.Right;
            if (robot is not Robot)
            {
                throw new Exception("Path does not start at a robot");
            }
            else
            {
                currentDirection = ((Robot)robot).Rotation;
            }

            for (int i = 1; i < path.Count; i++)
            {
                var startPos = path[i - 1].Position;
                var endPos = path[i].Position;
                var dir = startPos.DirectionInPosition(endPos);
                if (dir == null)
                {
                    continue;
                }
                else
                {
                    var ops = currentDirection.RotateTo((Direction)dir);
                    for (int j = 0; j < ops.Count; j++)
                    {
                        var op = ops[j];
                        switch (op)
                        {
                            case RobotOperation.Clockwise:
                                currentDirection = currentDirection.RotateClockWise();
                                break;
                            case RobotOperation.CounterClockwise:
                                currentDirection = currentDirection.RotateCounterClockWise();
                                break;
                            default:
                                break;
                        }
                        Operations.Enqueue(ops[j]);
                    }
                    Operations.Enqueue(RobotOperation.Forward);
                }
            }
            return Operations;
        }
        private List<Node> GetNeighbors(Node node,bool robotBlock)
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

                    if (checkX >= 0 && checkX < SimulationData!.Map.GetLength(0) && checkY >= 0 && checkY < SimulationData!.Map.GetLength(1) && (SimulationData.Map[checkX, checkY] is EmptyTile || robotBlock ? !(SimulationData.Map[checkX, checkY] is Robot) : SimulationData.Map[checkX, checkY] is Robot) )
                    {
                        neighbors.Add(new Node(new Position() { X = checkX, Y = checkY }));
                    }
                }
            }

            return neighbors;
        }



        private int Heuristic(Node a, Node b)
        {
            return Math.Abs(a.Position.X - b.Position.X) + Math.Abs(a.Position.X - b.Position.X);
        }

        private int EuclideanDistance(Node a, Node b)
        {
            return (int)Math.Sqrt(Math.Pow(a.Position.X - b.Position.X, 2) + Math.Pow(a.Position.X - b.Position.X, 2));
        }

        private void OnTaskFinished(RobotOperation[] result)
        {
            FinishedTask?.Invoke(this, new(result));
        }
        #endregion
        #region Node Class
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
        #endregion

    }
}
