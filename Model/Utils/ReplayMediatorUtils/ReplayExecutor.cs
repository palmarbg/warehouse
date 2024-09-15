using Model.Interfaces;
using Persistence.DataTypes;
using Persistence.Extensions;

namespace Model.Utils.ReplayMediatorUtils
{
    public class ReplayExecutor : IExecutor
    {
        private readonly SimulationData simulationData;
        public ReplayExecutor(SimulationData simulationData)
        {
            this.simulationData = simulationData;
        }

        public RobotOperation[] ExecuteOperations(RobotOperation[] robotOperations, float timeSpan)
        {
            for (int i = 0; i < simulationData.Robots.Count; i++)
            {
                Robot robot = simulationData.Robots[i];
                robot.NextOperation = robotOperations[i];
                robot.ExecuteMove();
            }

            return robotOperations;
        }

        public void SaveSimulationLog(string filepath)
        {
            throw new NotImplementedException();
        }

        public void Timeout()
        {

        }

        public IExecutor NewInstance(SimulationData simulationData)
        {
            return new ReplayExecutor(simulationData);
        }

        public void Dispose()
        {
            
        }
    }
}
