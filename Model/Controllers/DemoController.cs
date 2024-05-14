using Model.DataTypes;
using Model.Interfaces;
using Persistence.DataTypes;
using System.Diagnostics.CodeAnalysis;

namespace Model.Controllers
{
    [ExcludeFromCodeCoverage]
    public class DemoController : IController
    {
        private SimulationData? SimulationData;

        public event EventHandler<ControllerEventArgs>? FinishedTask;
        public event EventHandler? InitializationFinished;

        public string Name => "demo";

        public DemoController() { }

        /// <summary>
        /// Steps every Robot forward.
        /// Disregards time limit
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public void CalculateOperations(TimeSpan timeSpan, CancellationToken? token = null)
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

        public void InitializeController(SimulationData simulationData, TimeSpan timeSpan, ITaskDistributor distributor, CancellationToken? token = null)
        {
            SimulationData = simulationData;
            InitializationFinished?.Invoke(this, new());
        }

        private void OnTaskFinished(RobotOperation[] result)
        {
            FinishedTask?.Invoke(this, new(result));
        }

        public IController NewInstance()
        {
            return new DemoController();
        }
    }
}
