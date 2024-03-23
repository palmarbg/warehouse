using RobotokModel.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Distributors
{
    public class DemoDistributor : ITaskDistributor
    {
        private SimulationData simulationData;
        public DemoDistributor(SimulationData simulationData)
        {
            this.simulationData = simulationData;
        }

        /// <summary>
        /// Assignes the first available goal.
        /// If there is no available goal, assigns <c>null</c>
        /// </summary>
        /// <param name="robot"></param>
        public void AssignNewTask(Robot robot)
        {
            for(int i=0; i< simulationData.Goals.Count; i++)
            {
                Goal goal = simulationData.Goals[i];

                if(goal.IsAssigned)
                    continue;
                robot.CurrentGoal = goal;
                goal.IsAssigned = true;
                return;
            }
            robot.CurrentGoal = null;
        }
    }
}
