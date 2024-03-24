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
        private SimulationData? SimulationData;

        public event EventHandler<IControllerEventArgs>? FinishedTask;
        public string Name => "demo";
        
        public DemoController() { }

        /// <summary>
        /// Steps every Robot forward.
        /// Disregards time limit
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public void CalculateOperations(TimeSpan timeSpan)
        {
            if (SimulationData == null)
            {
                throw new InvalidOperationException();
            }

            RobotOperation[] result = new RobotOperation[SimulationData.Robots.Count];
            foreach (Robot robot in SimulationData.Robots)
            {
                robot.NextOperation = RobotOperation.Forward;
                result[robot.Id] = robot.NextOperation;
            }

            OnTaskFinished(result);
        }

        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan, ITaskDistributor distributor)
        {
            this.SimulationData = simulationData;
        }

        private void OnTaskFinished(RobotOperation[] result)
        {
            FinishedTask?.Invoke(this, new(result));
        }
    }
}
