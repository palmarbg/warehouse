using RobotokModel.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Controllers
{
    public class DemoController : IController
    {
        private SimulationData? simulationData;

        public event EventHandler<IControllerEventArgs>? FinishedTask;

        public DemoController() { }

        /// <summary>
        /// Steps every Robot forward.
        /// Disregards time limit
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public void ClaculateOperations(TimeSpan timeSpan)
        {
            //The method will be async, only for demo its omitted

            if (simulationData == null)
            {
                throw new InvalidOperationException();
            }

            RobotOperation[] result = new RobotOperation[Robot.Robots.Count];
            foreach(Robot robot in Robot.Robots)
            {
                robot.NextOperation = RobotOperation.Forward;
                result[robot.Id] = robot.NextOperation;
            }

            OnTaskFinished(result);
        }

        public void InitializeController(TimeSpan timeSpan, SimulationData simulationData)
        {
            this.simulationData = simulationData;
        }

        private void OnTaskFinished(RobotOperation[] result)
        {
            FinishedTask?.Invoke(this, new(result));
        }
    }
}
