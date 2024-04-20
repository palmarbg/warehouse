using Persistence.DataTypes;
using RobotokModel.Interfaces;
using System.Diagnostics;

namespace RobotokModel.Distributors
{
    public class DemoDistributor : ITaskDistributor
    {
        private SimulationData simulationData;
        private int iterator = 0;
        public event EventHandler<(Robot, Goal)>? TaskAssigned;
        public DemoDistributor(SimulationData simulationData)
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
            Debug.WriteLine(simulationData.Goals.Count);
            while (iterator < simulationData.Goals.Count)
            {
                Goal goal = simulationData.Goals[iterator];
                iterator++;

                if (goal.IsAssigned)
                    continue;

                robot.CurrentGoal = goal;
                goal.IsAssigned = true;
                OnTaskAssigned(robot);

                return;
            }
            robot.CurrentGoal = null;
        }

        public ITaskDistributor NewInstance(SimulationData simulationData)
        {
            return new DemoDistributor(simulationData);
        }

        private void OnTaskAssigned(Robot robot)
        {
            TaskAssigned?.Invoke(null, (robot, robot.CurrentGoal!));
        }
    }
}
