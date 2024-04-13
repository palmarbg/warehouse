using RobotokModel.Model.Extensions;
using RobotokModel.Model.Interfaces;
using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Executors
{
    public class DemoExecutor : IExecutor
    {
        private readonly SimulationData simulationData;
        public DemoExecutor(SimulationData simulationData)
        {
            this.simulationData = simulationData;
        }

        /// <summary>
        /// Doesn't handle deadlock, illegal moves.
        /// </summary>
        /// <param name="robotOperations"></param>
        public RobotOperation[] ExecuteOperations(RobotOperation[] robotOperations)
        {
            for (int i = 0; i < simulationData.Robots.Count; i++)
            {
                Robot robot = simulationData.Robots[i];
                robot.ExecuteMove();
            }
            
            return robotOperations ;
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
