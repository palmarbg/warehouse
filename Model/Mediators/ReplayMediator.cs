using Model.Interfaces;
using Model.Mediators.ReplayMediatorUtils;
using Persistence.DataAccesses;
using Persistence.Interfaces;
using System.Diagnostics;
using System.Numerics;

namespace Model.Mediators
{
    public class ReplayMediator : AbstractMediator, IReplayMediator
    {
        #region Private Fields

        private int? _savedInterval = null;

        #endregion
        #region Constructor

        public ReplayMediator(Simulation simulation, IServiceLocator serviceLocator) : base(simulation, serviceLocator)
        {

            Timer.Elapsed += (_, _) => StepSimulation();

            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("View"));

            var mapDataAccess = serviceLocator.GetConfigDataAccess(path + "sample_files\\random_20_config.json");//random_20_config
            dataAccess = _serviceLocator.GetLoadLogDataAccess(path + "sample_files\\random_20_log.json", mapDataAccess);//random_20_log

            simulationData = dataAccess.GetInitialSimulationData();

            controller = serviceLocator.GetReplayController((ILoadLogDataAccess)dataAccess);

            executor = _serviceLocator.GetReplayExecutor(simulationData);

        }

        public void StepForward()
        {
            if (
                simulationState.IsSimulationRunning ||
                simulationState.IsExecutingMoves
                )
                return;

            if (!simulationState.IsSimulationStarted)
            {
                InitSimulation();
                simulationState.IsSimulationEnded = false;
            }

            if(_savedInterval == null)
                _savedInterval = interval;
            interval = 100;

            StepSimulation(false);
        }

        public void StepBackward()
        {
            if (simulationState.IsSimulationRunning ||
                simulationState.IsExecutingMoves ||
                !simulationState.IsSimulationStarted)
                return;

            if (simulationData.Step < 1)
                return;

            simulationState.IsLastTaskFinished = false;

            if(_savedInterval == null)
                _savedInterval = interval;
            interval = 100;

            simulationData.Step--;
            //handle in service locator if null ref error
            (controller as IReplayController)!.CalculateBackward();
            simulationData.Step--;
        }

        public void JumpToStep(int step)
        {
            if (
                SimulationState.IsSimulationRunning ||
                SimulationState.IsExecutingMoves
                )
                return;

            if (step == simulationData.Step)
                return;

            if(step < SimulationData.Step || !SimulationState.IsSimulationStarted)
                InitSimulation();

            if(step == 0)
                return;

            simulationState.IsSimulationEnded = false;
            SimulationState.IsExecutingMoves = true;

            //handle in service locator if null ref error
            (controller as IReplayController)!.SetPosition(step);
            SimulationState.IsExecutingMoves = false;
            simulation.OnRobotsMoved(TimeSpan.Zero);
        }

        public void JumpToEnd()
        {
            if (
                SimulationState.IsSimulationRunning ||
                SimulationState.IsExecutingMoves
                )
                return;

            if (!SimulationState.IsSimulationStarted)
                InitSimulation();

            simulationState.IsSimulationEnded = false;
            SimulationState.IsExecutingMoves = true;

            //handle in service locator if null ref error
            (controller as IReplayController)!.JumpToEnd();

            SimulationState.IsExecutingMoves = false;
            simulation.OnRobotsMoved(TimeSpan.Zero);
        }

        public void SetSpeed(float speed)
        {
            int calculatedInterval = (int)(1000 / speed);

            if (calculatedInterval <= 0)
                return;

            if (_savedInterval != null)
                _savedInterval = calculatedInterval;
            else
                interval = calculatedInterval;
            
            Timer.Interval = calculatedInterval;
        }

        #endregion

        #region Private methods

        private void StepSimulation(bool? toDeleteInterval = null)
        {
            Debug.WriteLine(SimulationData.Step);
            Debug.WriteLine("--SIMULATION STEP--");

            if (simulationState.IsExecutingMoves)
                return;

            if (!simulationState.IsLastTaskFinished)
            {
                OnTaskTimeout();
                return;
            }

            simulationState.IsLastTaskFinished = false;

            if (toDeleteInterval == null && _savedInterval != null)
            {
                interval = (int)_savedInterval;
                _savedInterval = null;
            }

            controller.CalculateOperations(TimeSpan.FromMilliseconds(interval));

        }

        private void OnTaskTimeout()
        {
            Debug.WriteLine("XXXX TIMEOUT XXXX");
            executor.Timeout();
        }

        #endregion
    }
}
