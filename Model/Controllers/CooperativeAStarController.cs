using Model.DataTypes;
using Model.Interfaces;
using Persistence.DataTypes;
using Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Controllers
{
    [ExcludeFromCodeCoverage]
    public class CooperativeAStarController : IController
    {
        public string Name => "CoopAStarController";
        private SimulationData? SimulationData = null!;
        private ITaskDistributor _taskDistributor = null!;
        private List<Queue<RobotOperation>> _plannedOperations = new List<Queue<RobotOperation>>();

        private bool[] _robotsFinished = [];
        private RobotOperation[] _previousOperations = [];
        private int[] _blockedCount = [];
        private List<Reserver?>? _reservers = null!;
        private Dictionary<Position, SortedList<int, Reserver>>? _reservations = null!;
        private int _currentTurn = 0;

        public event EventHandler<IControllerEventArgs>? FinishedTask;
        public event EventHandler? InitializationFinished;

        #region Public Methods
        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan, ITaskDistributor distributor, CancellationToken? token = null)
        {
            _taskDistributor = distributor;
            SimulationData = simulationData;
            _robotsFinished = new bool[simulationData.Robots.Count];
            _previousOperations = new RobotOperation[simulationData.Robots.Count];
            _blockedCount = new int[simulationData.Robots.Count];

            _currentTurn = 0;
            _reservers = new List<Reserver?>();
            _reservations = new Dictionary<Position, SortedList<int, Reserver>>();


            for (int i = 0; i < simulationData.Robots.Count; i++)
            {
                if (simulationData.Robots[i].CurrentGoal is null) _taskDistributor.AssignNewTask(simulationData.Robots[i]);
                if (simulationData.Robots[i].CurrentGoal is null) _robotsFinished[i] = true;
                else _robotsFinished[i] = false;
            }

            simulationData.Robots.ForEach(robot =>
            {
                if (robot.CurrentGoal is null) _taskDistributor.AssignNewTask(robot);
                var reserver = new Reserver(robot);
                _reservers.Add(reserver);
            });

            _plannedOperations = new List<Queue<RobotOperation>>();
            for (int i = 0; i < SimulationData.Robots.Count; i++)
            {
                if (token != null && ((CancellationToken)token)!.IsCancellationRequested)
                {
                    return;
                }
                _plannedOperations.Add(FindPath(SimulationData.Robots[i]));
            }
            InitializationFinished?.Invoke(this, new());
        }
        public IController NewInstance()
        {
            return new CooperativeAStarController();
        }

        //TODO: Deadlock : refactor
        public void CalculateOperations(TimeSpan timeSpan, CancellationToken? token = null)
        {
            if (_currentTurn == 500)
            {
            }
            if (SimulationData is null) { throw new Exception("initialize the controller first"); }
            var result = new List<RobotOperation>();
            for (int i = 0; i < _plannedOperations.Count; i++)
            {
                if (token != null && ((CancellationToken)token)!.IsCancellationRequested)
                {
                    Debug.WriteLine("CANCEL");
                    Debug.WriteLine("CANCEL");
                    return;
                }
                var robot = SimulationData.Robots[i];
                // nincs utasítás a robot számára
                if (_plannedOperations[i].Count == 0 || robot.CurrentGoal is null)
                {
                    if (robot.CurrentGoal is null)
                    {
                        if (_taskDistributor.AllTasksAssigned)
                        {
                            // TODO ha taskon áll el lehet küldeni valahova máshova
                            _robotsFinished[i] = true;
                            result.Add(RobotOperation.Wait);
                            robot.NextOperation = RobotOperation.Wait;
                        }
                        // Új task
                        else
                        {
                            _taskDistributor.AssignNewTask(robot);
                            _plannedOperations[i] = FindPath(robot);
                            if (_plannedOperations[i].Count == 0)
                            {
                                AddOperation(result, robot, RobotOperation.Wait, i);
                                _blockedCount[robot.Id]++;
                            }
                            else
                            {
                                AddOperation(result, robot, _plannedOperations[i].Dequeue(), i);
                            }
                        }
                    }
                    //Van task de nincs út
                    else
                    {
                        _plannedOperations[i] = FindPath(robot, false);
                        if (_plannedOperations[i].Count == 0)
                        {
                            AddOperation(result, robot, RobotOperation.Wait, i);
                            _blockedCount[robot.Id]++;
                        }
                        else
                        {
                            AddOperation(result, robot, _plannedOperations[i].Dequeue(), i);
                        }
                    }
                }
                else
                {
                    //TODO : szebb deadlock
                    if (robot.BlockedThisTurn)
                    {
                        _plannedOperations[i] = FindPath(robot, false);
                        _blockedCount[i] = 0;
                        if (_plannedOperations[i].Count == 0)
                        {
                            AddOperation(result, robot, RobotOperation.Wait, i);
                            _blockedCount[robot.Id]++;
                        }
                        else
                        {
                            AddOperation(result, robot, _plannedOperations[i].Dequeue(), i);
                        }
                        continue;
                    }
                    else
                    {
                        AddOperation(result, robot, _plannedOperations[i].Dequeue(), i);
                    }
                }
            }

            _currentTurn++;
            OnTaskFinished([.. result]);
        }
        private void AddOperation(List<RobotOperation> result, Robot robot, RobotOperation nextOp, int index)
        {
            robot.NextOperation = nextOp;
            _previousOperations[index] = nextOp;
            result.Add(nextOp);
        }
        #endregion
        #region Private Methods
        private Queue<RobotOperation> FindPath(Robot robot, bool robotBlock = false)
        {
            if (robot.CurrentGoal is null) { throw new Exception("Robot does not have a goal!"); }
            if (_reservations is null) { throw new Exception("Initialize the controller before using it!"); }
            DeleteReservation(_reservers![robot.Id]!);
            HashSet<Node> openSet = new HashSet<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            Dictionary<Node, int> gScore = new Dictionary<Node, int>();
            Dictionary<Node, int> fScore = new Dictionary<Node, int>();
            var start = new Node(robot.Position);
            start.Direction = robot.Rotation;
            var goal = new Node(robot.CurrentGoal.Position);
            openSet.Add(start);
            start.Turn = _currentTurn;
            start.AllowedRotations = AllowedRotationsOnPosition(start.Position, start.Turn);
            gScore[start] = 0;
            fScore[start] = Heuristic(start, goal);

            while (openSet.Count > 0)
            {
                Node current = null!;
                int lowestFScore = int.MaxValue;
                foreach (var node in openSet)
                {
                    if (fScore.TryGetValue(node, out int value) && value < lowestFScore)
                    {
                        lowestFScore = value;
                        current = node;
                    }
                }
                if (current.Direction is null) throw new Exception();


                if (current.Position.EqualsPosition(goal.Position))
                {
                    return RetracePath(start, current);
                }

                closedSet.Add(current);
                openSet.Remove(current);

                foreach (var neighbor in GetNeighbors(current, robotBlock))
                {
                    var directionOnNewNode = (Direction)current.Position.DirectionInPosition(neighbor.Position)!;
                    int rotationCost = ((Direction)current.Direction).RotateTo(directionOnNewNode).Count;

                    int tentativeGScore = gScore[current] + 1 + rotationCost;

                    int freeTurns = AllowedRotationsOnPosition(neighbor.Position, current.Turn + 1 + rotationCost);

                    if ((!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                        && freeTurns >= 0
                        && current.AllowedRotations >= rotationCost
                        && !Collides(current.Position, neighbor.Position, current.Turn + rotationCost)
                        && (!goal.Position.EqualsPosition(neighbor.Position) || freeTurns == 2)
                        )
                    {
                        neighbor.parent = current;
                        neighbor.AllowedRotations = freeTurns;
                        neighbor.Turn = current.Turn + rotationCost + 1;
                        gScore[neighbor] = tentativeGScore;
                        neighbor.Direction = directionOnNewNode;
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

        private bool Collides(Position sourcePos, Position destPos, int turn)
        {
            if (FrontalCollides(sourcePos, destPos, turn))
            {
                return true;
            }
            else if (SideCollides(sourcePos, destPos, turn))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SideCollides(Position sourcePos, Position destPos, int turn)
        {
            return _reservations!.ContainsKey(destPos)
                && _reservations[destPos].ContainsKey(turn + 1);
        }

        // [robot]-><-[robot]
        private bool FrontalCollides(Position sourcePos, Position destPos, int turn)
        {
            return _reservations!.ContainsKey(destPos) &&
                _reservations[destPos].ContainsKey(turn) &&
                _reservations.ContainsKey(sourcePos) &&
                _reservations[sourcePos].ContainsKey(turn + 1)
                 && _reservations[sourcePos][turn + 1].Robot.Id == _reservations[destPos][turn].Robot.Id;
        }
        private int AllowedRotationsOnPosition(Position pos, int turn)
        {
            if (_reservations!.ContainsKey(pos))
            {
                if (_reservations[pos].ContainsKey(turn) && _reservations[pos][turn] is not null)
                {
                    return -1;
                }
                else if (_reservations[pos].ContainsKey(turn + 1) && _reservations[pos][turn + 1] is not null)
                {
                    return 0;
                }
                else if (_reservations[pos].ContainsKey(turn + 2) && _reservations[pos][turn + 2] is not null)
                {
                    return 1;
                }
                else if (_reservations[pos].ContainsKey(turn + 3) && _reservations[pos][turn + 3] is not null)
                {
                    return 2;
                }
            }
            return 2;
        }
        private List<Node> GetNeighbors(Node node, bool robotBlock)
        {
            List<Node> neighbors = new List<Node>();

            for (int xOffset = -1; xOffset <= 1; xOffset++)
            {
                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    if (xOffset != 0 && yOffset != 0 || xOffset == 0 && yOffset == 0)
                        continue;

                    int checkX = node.Position.X + xOffset;
                    int checkY = node.Position.Y + yOffset;

                    if (checkX >= 0 && checkX < SimulationData!.Map.GetLength(0) && checkY >= 0 && checkY < SimulationData!.Map.GetLength(1)
                        && SimulationData.Map[checkX, checkY] is not Block
                        && (SimulationData.Map[checkX, checkY] is EmptyTile || (robotBlock ? SimulationData.Map[checkX, checkY] is not Robot : SimulationData.Map[checkX, checkY] is Robot))
                        && !(SimulationData.Map[checkX, checkY] is Robot robot && _robotsFinished[robot.Id]))
                    {
                        neighbors.Add(new Node(new Position() { X = checkX, Y = checkY }));
                    }
                }
            }

            return neighbors;
        }
        private Queue<RobotOperation> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;
            if (currentNode is null || currentNode.parent is null && currentNode != startNode) { throw new Exception("Unexpected error in Pathfinding"); }

            while (currentNode != startNode)
            {
                path.Add(currentNode!);
                currentNode = currentNode.parent!;
            }
            path.Add(startNode);

            path.Reverse();
            return NodePathToRobotOperationList(path);
        }

        /// <summary>
        /// Converts Node List to RobotOperation List. Reserves Positions for the robot
        /// </summary>
        /// <param name="path"> List of Nodes in the path</param>
        /// <returns> List of Operations needed to complete path</returns>
        /// <exception cref="Exception"></exception>
        private Queue<RobotOperation> NodePathToRobotOperationList(List<Node> path)
        {
            if (SimulationData is null) { throw new Exception("initialize the controller first"); }
            Queue<RobotOperation> Operations = new Queue<RobotOperation>();
            var startTile = SimulationData.Map.GetAtPosition(path[0].Position);
            Direction currentDirection;

            if (startTile is not Robot)
            {
                throw new Exception("Path does not start at a robot");
            }

            var robot = (startTile as Robot)!;
            currentDirection = (robot).Rotation;

            var reserver = _reservers![robot.Id]!;
            int turn = path[0].Turn;

            if (robot.Id == 18)
            {

            }
            if(path.Count < 2)
            {
                Operations.Enqueue(RobotOperation.Wait);
                return Operations;
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
                        turn++;
                        Reserve(startPos, turn, reserver);
                    }
                    Operations.Enqueue(RobotOperation.Forward);
                    turn++;
                    Reserve(endPos, turn, reserver);
                }

            }
            if (path.Count > 1)
            {
                turn++;
                Reserve(path.Last().Position, turn, reserver);
                turn++;
                Reserve(path.Last().Position, turn, reserver);
            }
            return Operations;
        }

        private void DeleteReservation(Reserver reserver)
        {
            var res = new Dictionary<Position, SortedList<int, Reserver>>();
            foreach (var item in _reservations!)
            {
                var pos = item.Key;

                res.Add(pos, []);
                foreach (var ress in item.Value)
                {
                    if (ress.Value.Robot.Id != reserver.Robot.Id)
                    {
                        res[pos].Add(ress.Key, ress.Value);
                    }

                }
                if (res[pos].Count == 0) res.Remove(pos);
            }
            _reservations = res;
        }

        private void Reserve(Position position, int turn, Reserver reserver)
        {
            if (_reservations is null)
            {
                _reservations = new Dictionary<Position, SortedList<int, Reserver>>();
            }
            if (!_reservations.ContainsKey(position))
            {
                _reservations.Add(position, []);
            }
            _reservations[position].Add(turn, reserver);
        }


        private int Heuristic(Node a, Node b)
        {
            int dx = Math.Abs(a.Position.X - b.Position.X);
            int dy = Math.Abs(a.Position.Y - b.Position.Y);
            int turns = 0;
            if (a.Position.X != b.Position.X)
            {
                turns += 1;
            }
            if (a.Position.Y != b.Position.Y)
            {
                turns += 1;
            }
            if (a.Position.DirectionInPosition(b.Position) != a.Direction)
            {
                turns += 1;
            }
            return dx + dy + turns;
        }

        private void OnTaskFinished(RobotOperation[] result)
        {
            FinishedTask?.Invoke(this, new(result));
        }
        #endregion


    }
    [ExcludeFromCodeCoverage]
    public class Reserver
    {
        public Robot Robot;
        public int Offset;
        public Reserver(Robot robot)
        {
            Robot = robot;
            Offset = 0;
        }
        public override string ToString()
        {
            return this.Robot.Id.ToString();
        }

    }

}
