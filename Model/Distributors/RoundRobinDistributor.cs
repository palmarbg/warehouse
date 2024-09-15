﻿using Model.Interfaces;
using Persistence.DataTypes;

namespace Model.Distributors
{
    public class RoundRobinDistributor : ITaskDistributor
    {
        private SimulationData simulationData;
        private int iterator = 0;

        public RoundRobinDistributor(SimulationData simulationData)
        {
            this.simulationData = simulationData;
        }

        public bool AllTasksAssigned => iterator == simulationData.Goals.Count;

        /// <summary>
        /// Assignes the first available goal.
        /// If there is no available goal, assigns <c>null</c>
        /// </summary>
        /// <param name="robot"></param>
        public void AssignNewTask(Robot robot)
        {
            while (iterator < simulationData.Goals.Count)
            {
                Goal goal = simulationData.Goals[iterator];
                iterator++;

                if (goal.IsAssigned)
                    continue;

                goal.IsAssigned = true;
                robot.CurrentGoal = goal;

                return;
            }
            robot.CurrentGoal = null;
        }

        public ITaskDistributor NewInstance(SimulationData simulationData)
        {
            return new RoundRobinDistributor(simulationData);
        }
    }
}
