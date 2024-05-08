using Model.Interfaces;
using Persistence.DataTypes;
using Persistence.Extensions;

namespace Model.Executors
{
    public class DemoExecutor : IExecutor
    {
        private readonly SimulationData simulationData;
        public DemoExecutor(SimulationData simulationData)
        {
            this.simulationData = simulationData;
        }

        public void Dispose()
        {
            
        }

        /// <summary>
        /// Doesn't handle deadlock, illegal moves.
        /// </summary>
        /// <param name="robotOperations"></param>
        public RobotOperation[] ExecuteOperations(RobotOperation[] robotOperations, float timeSpan)
        {
            for (int i = 0; i < simulationData.Robots.Count; i++)
            {
                Robot robot = simulationData.Robots[i];
                robot.ExecuteMove();
            }

            return robotOperations;
        }

        public IExecutor NewInstance(SimulationData simulationData)
        {
            return new DemoExecutor(simulationData);
        }

        public void SaveSimulation(string filepath)
        {
            throw new NotImplementedException();
        }

        public void TaskAssigned(int taskId, int robotId)
        {
            throw new NotImplementedException();
        }

        public void Timeout()
        {
            throw new NotImplementedException();
        }
    }
}
