using Model.Interfaces;
using Model.Mediators.ReplayMediatorUtils;
using Persistence.Interfaces;
using System.Diagnostics;

namespace Model.Mediators
{
    public class ReplayMediator : AbstractMediator, IReplayMediator
    {
        #region Private Fields

        private int? _savedInterval = null;

        #endregion

        #region Public Fields

        public override int Interval => _savedInterval ?? interval;

        #endregion

        #region Constructor

        public ReplayMediator(ISimulation simulation, IServiceLocator serviceLocator, string mapFileName, string logFileName) : base(simulation, serviceLocator, mapFileName)
        {

            Timer.Elapsed += (_, _) => StepSimulation();

            var mapDataAccess = serviceLocator.GetConfigDataAccess(mapFileName);
            
            dataAccess = _serviceLocator.GetLoadLogDataAccess(logFileName, mapDataAccess);

            simulationData = dataAccess.GetInitialSimulationData();

            controller = serviceLocator.GetReplayController((ILoadLogDataAccess)dataAccess);

            executor = _serviceLocator.GetReplayExecutor(simulationData);

        }

        public void LoadLog(string fileName)
        {
            dataAccess = dataAccess.NewInstance(fileName);
            SetInitialState();
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

            if (_savedInterval == null)
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

            if (_savedInterval == null)
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

            if (step < SimulationData.Step || !SimulationState.IsSimulationStarted)
                InitSimulation();

            if (step == 0)
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
