using Model.Interfaces;
using Persistence.DataTypes;
using Persistence.Extensions;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Executors
{
    public class StrictExecutor : ExecutorBase, IExecutor
    {
        private EventHandler<Goal> _robotTaskAssignedDelegate;
        private Dictionary<Position, RobotNode> _nodes = null!;
        private HashSet<Node> _headNodes = null!;
        private RobotOperation[] _executedOperations = null!;
        private RobotNode _lastLoopEndNode = null!;

        public StrictExecutor(SimulationData simulationData, ILogger logger) : base(simulationData, logger)
        {
            _robotTaskAssignedDelegate = (robot, goal) => OnTaskAssigned(goal.Id, ((Robot)robot!).Id);

            Robot.TaskAssigned += _robotTaskAssignedDelegate;
        }

        public RobotOperation[] ExecuteOperations(RobotOperation[] robotOperations, float timeSpan)
        {
            Debug.WriteLine("Executing");
            _errors = new List<OperationError>();
            _nodes = new Dictionary<Position, RobotNode>();
            _headNodes = new HashSet<Node>();
            _executedOperations = new RobotOperation[robotOperations.Length];

            for (int i = 0; i < _simulationData.Robots.Count; i++)
            {
                _simulationData.Robots[i].MovedThisTurn = false;
                _simulationData.Robots[i].InspectedThisTurn = false;
                _simulationData.Robots[i].BlockedThisTurn = false;
            }

            for (int i = 0; i < _simulationData.Robots.Count; i++)
            {
                var robot = _simulationData.Robots[i];
                if (robot.InspectedThisTurn)
                    continue;
                AddRobotNode(robot);
                if(_lastLoopEndNode != null)
                {
                    Debug.WriteLine(_lastLoopEndNode.Robot.Id);

                    throw new NotImplementedException("Its a bug1");
                }
            }

            foreach (var node in _headNodes)
            {
                ExecuteNode(node);
            }

            for (int i = 0; i < _simulationData.Robots.Count; i++)
            {
                var robot = _simulationData.Robots[i];

                if (!robot.MovedThisTurn)
                    throw new NotImplementedException("Its a bug2");

                if(_simulationData.Map.GetAtPosition(robot.Position) is Robot robotOnMap)
                if(robotOnMap.Id == robot.Id)
                {
                    _simulationData.Map.SetAtPosition(robot.Position, EmptyTile.Instance);
                }

                robot.ExecuteMove(_executedOperations[robot.Id]);

                _simulationData.Map.SetAtPosition(robot.Position, robot);
                
                if (robot.CurrentGoal == null)
                    continue;
                
                if (robot.CurrentGoal.Position.Equals(robot.Position))
                {
                    OnTaskFinished(robot.CurrentGoal.Id, robot.Id);
                    robot.CurrentGoal = null;
                }
            }

            OnStepFinished(robotOperations, _executedOperations, _errors.ToArray(), timeSpan);
            Debug.WriteLine("STOP Executing");
            return robotOperations;
        }

        /// <summary>
        /// Supposes that robotOperations[?] == robot.NextOparation for all robots
        /// </summary>
        /// <param name="robot"></param>
        private void AddRobotNode(Robot robot)
        {
            //Debug.WriteLine("AddRobotNode");

            if (robot.InspectedThisTurn)
                return;
            robot.InspectedThisTurn = true;

            var nextPosition = robot.Position.PositionAfterOperation(robot.Rotation, robot.NextOperation);

            var node = new RobotNode
            {
                Robot = robot,
                Position = robot.Position,
                NextPosition = nextPosition
            };

            if(robot.NextOperation == RobotOperation.Backward)
                throw new InvalidOperationException();

            if(robot.NextOperation != RobotOperation.Forward)
            {
                robot.MovedThisTurn = true;
                robot.BlockedThisTurn = false;
                _executedOperations[robot.Id] = robot.NextOperation;
                return;
            }

            //INVARIANT: robot operation is Forward

            //if robot wants to go out of the map
            if( !_simulationData.Map.IsOnMap(nextPosition) )
            {
                robot.MovedThisTurn = true;
                robot.BlockedThisTurn = true;
                _executedOperations[robot.Id] = RobotOperation.Wait;
                OnWallHit(robot.Id);
                return;
            }

            //if robot hits a block
            if (_simulationData.Map.GetAtPosition(nextPosition) is Block)
            {
                robot.MovedThisTurn = true;
                robot.BlockedThisTurn = true;
                _executedOperations[robot.Id] = RobotOperation.Wait;
                OnWallHit(robot.Id);
                return;
            }

            //if wants to step to an empty tile that was already visited
            if (_headNodes.Contains(new Node { Position = node.NextPosition}))
            {
                Node? parent;
                _headNodes.TryGetValue(new Node { Position = node.NextPosition }, out parent);

                if(parent == null)
                    throw new NotImplementedException("Its a bug 4");

                parent.ChildNodes.Add(node);
                _nodes.Add(node.Position, node);
                return;
            }

            //if wants to step on a robot that was already visited
            if (_nodes.ContainsKey(node.NextPosition))
            {
                var parent = _nodes[node.NextPosition];
                parent.ChildNodes.Add(node);
                _nodes.Add(node.Position, node);
                return;
            }

            //if robot steps to an empty tile
            //invariant: this tile was not visited yet
            if (_simulationData.Map.GetAtPosition(nextPosition) is EmptyTile)
            {
                _nodes.Add(node.Position, node);
                var headNode = new Node
                {
                    Position = nextPosition
                };
                headNode.ChildNodes.Add(node);
                _headNodes.Add(headNode);
                return;
            }

            //if robot follows a robot
            //invariant: the robot was not visited yet or could not move
            if (_simulationData.Map.GetAtPosition(nextPosition) is Robot followedRobot)
            {
                AddRobotNode(followedRobot);
                if (followedRobot.BlockedThisTurn || followedRobot.NextOperation != RobotOperation.Forward)
                {
                    robot.MovedThisTurn = true;
                    robot.BlockedThisTurn = true;
                    _executedOperations[robot.Id] = RobotOperation.Wait;
                    // TODO : does it count as a crash?
                    OnRobotCrash(robot.Id, followedRobot.Id);
                    return;
                }

                _nodes.Add(node.Position, node);

                //if its a head of a loop
                if (_headNodes.Contains(node))
                {
                    node.ChildNodes.Add(_lastLoopEndNode);
                    _lastLoopEndNode = null!;
                }
                
                //if its a new loop
                if (!_nodes.ContainsKey(nextPosition))
                {
                    //frontal crash = 2 robot loop
                    if (followedRobot.Position.PositionInDirection(followedRobot.Rotation).Equals(node.Position))
                    {
                        robot.MovedThisTurn = true;
                        robot.BlockedThisTurn = true;
                        _executedOperations[robot.Id] = RobotOperation.Wait;
                        OnRobotCrash(robot.Id, followedRobot.Id);
                        return;
                    }

                    if (_lastLoopEndNode != null)
                        throw new NotImplementedException("Its a bug3");

                    _lastLoopEndNode = node;
                    var headNode = new Node
                    {
                        Position = nextPosition
                    };
                    headNode.ChildNodes.Add(node);
                    _headNodes.Add(headNode);
                    return;
                }

                var parent = _nodes[node.NextPosition];
                parent.ChildNodes.Add(node);
                return;
            }

            throw new NotImplementedException();
        }

        private void ExecuteNode(Node head)
        {
            //Debug.WriteLine("ExecuteNode");
            if(head.ChildNodes.Count > 1)
            {
                LogMultipleCollision(head.ChildNodes);
                while (head.ChildNodes.Count > 0)
                {
                    var child = head.ChildNodes[head.ChildNodes.Count - 1];
                    head.ChildNodes.RemoveAt(head.ChildNodes.Count - 1);
                    DisableNode(child);
                }
                return;
            }

            var node = head.ChildNodes.First();

            var robot = node.Robot;

            while (!robot.MovedThisTurn)
            {
                robot.MovedThisTurn = true;
                robot.BlockedThisTurn = false;
                _executedOperations[robot.Id] = robot.NextOperation;

                if (node.ChildNodes.Count == 0)
                    return;

                if(node.ChildNodes.Count > 1)
                {
                    LogMultipleCollision(node.ChildNodes);
                    while (node.ChildNodes.Count > 0)
                    {
                        var child = node.ChildNodes[node.ChildNodes.Count - 1];
                        node.ChildNodes.RemoveAt(node.ChildNodes.Count - 1);
                        DisableNode(child);
                    }
                    return;
                }

                node = node.ChildNodes[0];
                robot = node.Robot;
            };
        }

        private void LogMultipleCollision(List<RobotNode> childNodes)
        {
            for(int i = 0; i < childNodes.Count; i++)
            for(int j = i+1; j < childNodes.Count; j++)
                {
                    OnRobotCrash(childNodes[i].Robot.Id, childNodes[j].Robot.Id);
                }
        }

        private void DisableNode(RobotNode node)
        {
            var robot = node.Robot;
            robot.MovedThisTurn = true;
            robot.BlockedThisTurn = true;
            _executedOperations[robot.Id] = RobotOperation.Wait;

            while(node.ChildNodes.Count > 0)
            {
                var child = node.ChildNodes[node.ChildNodes.Count - 1];
                node.ChildNodes.RemoveAt(node.ChildNodes.Count - 1);
                DisableNode(child);
            }
        }

        public void Timeout()
        {
            OnTimeout();
        }

        public void SaveSimulationLog(string filepath)
        {
            _logger.SaveLog(filepath);
        }

        public IExecutor NewInstance(SimulationData simulationData)
        {
            return new StrictExecutor(simulationData, _logger.NewInstance(simulationData));
        }

        public void Dispose()
        {
            Robot.TaskAssigned -= _robotTaskAssignedDelegate;
        }


        private class RobotNode : Node
        {
            /// <summary>
            /// The position where the controller sends the robot.
            /// </summary>
            public required Position NextPosition { get; init; }

            /// <summary>
            /// The robot it represents.
            /// </summary>
            public required Robot Robot { get; init; }

            public bool IsVisited { get; set; } = false;
        }

        private class Node
        {
            /// <summary>
            /// The starting position of the robot it represents.
            /// </summary>
            public required Position Position { get; init; }

            /// <summary>
            /// Nodes, which robots want to get to this node.
            /// </summary>
            public List<RobotNode> ChildNodes { get; } = new List<RobotNode>();

            public override bool Equals(object? obj)
            {
                if(obj is Node node)
                {
                    return Position.Equals(node.Position);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Position.GetHashCode();
            }
        }
    }
}
