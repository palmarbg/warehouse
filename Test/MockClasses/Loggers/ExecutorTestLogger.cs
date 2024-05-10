using Persistence.DataTypes;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.MockClasses.Loggers
{
    public class ExecutorTestLogger(SimulationData simulationData) : ILogger
    {
        private SimulationData _simulationData = simulationData;
        private List<string> _steps = new List<string>();

        public void LogEvent(TaskEvent taskEvent, int robotId)
        {
            throw new InvalidOperationException();
        }

        public void LogStep(RobotOperation[] controllerOperations, RobotOperation[] robotOperations, OperationError[] errors, float timeElapsed)
        {
            _steps.Add(SerializeSimulationData());
        }

        public void LogTimeout()
        {
            throw new InvalidOperationException();
        }

        public ILogger NewInstance(SimulationData simulationData)
        {
            throw new InvalidOperationException(); 
        }

        public void SaveLog(string path)
        {
            throw new InvalidOperationException();
        }

        public void SaveToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string line in _steps)
                {
                    writer.WriteLine(line);
                }
            }
        }

        private string SerializeSimulationData()
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < _simulationData.Map.GetLength(1); y++) 
            {
                for (int x = 0; x < _simulationData.Map.GetLength(0); x++)
                {
                    var tile = _simulationData.Map[x, y];
                    if (tile is EmptyTile)
                    {
                        sb.Append(".");
                    }
                    else if (tile is Block)
                    {
                        sb.Append("X");
                    }
                    else if (tile is Robot robot)
                    {
                        switch (robot.Rotation)
                        {
                            case Direction.Right:
                                sb.Append("R");
                                break;
                            case Direction.Down:
                                sb.Append("D");
                                break;
                            case Direction.Left:
                                sb.Append("L");
                                break;
                            case Direction.Up:
                                sb.Append("U");
                                break;
                            default:
                                sb.Append("?");
                                break;
                        }
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
