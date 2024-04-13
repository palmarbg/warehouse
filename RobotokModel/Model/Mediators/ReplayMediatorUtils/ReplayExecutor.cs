using RobotokModel.Model.Executors;
using RobotokModel.Model.Extensions;
using RobotokModel.Model.Interfaces;
using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Mediators.ReplayMediatorUtils
{
    public class ReplayExecutor : IExecutor
    {
        private readonly SimulationData simulationData;
        public ReplayExecutor(SimulationData simulationData)
        {
            this.simulationData = simulationData;
        }

        public RobotOperation[] ExecuteOperations(RobotOperation[] robotOperations)
        {
            for (int i = 0; i < simulationData.Robots.Count; i++)
            {
                Robot robot = simulationData.Robots[i];
                robot.NextOperation = robotOperations[i];
                robot.ExecuteMove();
            }

            return robotOperations;
        }

        public IExecutor NewInstance(SimulationData simulationData)
        {
            return new ReplayExecutor(simulationData);
        }

        public void SaveSimulation(string filepath)
        {
            throw new NotImplementedException();
        }

        public void Timeout()
        {
            
        }
    }
}
