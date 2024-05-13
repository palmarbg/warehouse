using Model.DataTypes;
using Model.Interfaces;
using Model.Utils.ReplayMediatorUtils;
using Persistence.Interfaces;
using System.Diagnostics;

namespace Model.Mediators
{
    public class ReplayMediator : MediatorBase, IReplayMediator
    {
        #region Private Fields

        private int? _savedInterval = null;

        #endregion

        #region Public Fields

        public override int Interval => _savedInterval ?? _interval;

        #endregion

        #region Constructor

        public ReplayMediator(ISimulation simulation, IServiceLocator serviceLocator, string mapFileName, string logFileName) : base(simulation, serviceLocator, mapFileName)
        {

            Timer.Elapsed += (_, _) => OnTimerInterval();

            var mapDataAccess = serviceLocator.GetConfigDataAccess(mapFileName);
            
            _dataAccess = _serviceLocator.GetLoadLogDataAccess(logFileName, mapDataAccess);

            _simulationData = _dataAccess.GetInitialSimulationData();

            _controller = serviceLocator.GetReplayController((ILoadLogDataAccess)_dataAccess);
            _lastStep = (_controller as IReplayController)!.GetSimulationLength();

            _executor = _serviceLocator.GetReplayExecutor(_simulationData);


        }

        #endregion

        #region Public Methods

        public void LoadLog(string fileName)
        {
            _dataAccess = _dataAccess.NewInstance(fileName);
            SetInitialPosition();
            //handle in service locator if you get null ref error
            _lastStep = (_controller as IReplayController)!.GetSimulationLength();
        }

        public void StepForward()
        {
            if (_simulationState.IsSimulationRunning)
                return;

            InitSimulationIfNeeded();

            SaveInterval();
            _interval = 100;

            OnTimerInterval(false);
        }

        public void StepBackward()
        {
            if(_simulationState.IsSimulationRunning)
                return;

            if (_simulationData.Step <= 0)
                return;

            SimulationState.State = SimulationStates.SimulationPaused;

            SaveInterval();
            _interval = 100;

            _simulationData.Step--;
            //handle in service locator if you get null ref error
            (_controller as IReplayController)!.CalculateBackward();
        }

        public void JumpToStep(int step)
        {
            if(_simulationState.IsSimulationRunning)
                return;

            //we should go backward
            if (step < SimulationData.Step)
                SetInitialPosition();

            //we are at the right step
            if (step == _simulationData.Step)
                return;
            
            InitSimulationIfNeeded();

            //handle in service locator if you get null ref error
            (_controller as IReplayController)!.SetPosition(step);
            _simulation.OnRobotsMoved(new RobotsMovedEventArgs()
            {
                SimulationStep = _simulationData.Step,
                IsJumped = true,
                TimeSpan = TimeSpan.Zero,
            });
        }

        public void JumpToEnd()
        {
            if(_simulationState.IsSimulationRunning)
                return;

            InitSimulationIfNeeded();

            //handle in service locator if you get null ref error
            (_controller as IReplayController)!.JumpToEnd();
            _simulation.OnRobotsMoved(new RobotsMovedEventArgs()
            {
                SimulationStep = _simulationData.Step,
                IsJumped = true,
                TimeSpan = TimeSpan.Zero,
            });
        }

        public void SetSpeed(float speed)
        {
            int calculatedInterval = (int)(1000 / speed);

            if (calculatedInterval <= 0)
                return;

            if (_savedInterval != null)
                _savedInterval = calculatedInterval;
            else
                _interval = calculatedInterval;

            Timer.Interval = calculatedInterval;
        }

        #endregion

        #region Private methods

        private void OnTimerInterval(bool? toRestoreInterval = null)
        {
            Debug.WriteLine("--SIMULATION STEP--");

            if (_simulationData.Step >= _lastStep)
            {
                PauseSimulation();
                return;
            }

            if (_simulationState.State == SimulationStates.ControllerWorking)
            {
                OnTaskTimeout();
                return;
            }

            if (!(
                    _simulationState.State == SimulationStates.Waiting ||
                    _simulationState.State == SimulationStates.SimulationPaused
                ))
                return;

            _simulationState.State = SimulationStates.ControllerWorking;

            if (toRestoreInterval != false)
                RestoreInterval();

            _controller.CalculateOperations(TimeSpan.FromMilliseconds(_interval));

        }

        private void OnTaskTimeout()
        {
            Debug.WriteLine("XXXX TIMEOUT XXXX");
            _executor.Timeout();
        }

        private void SaveInterval()
        {
            if (_savedInterval == null)
                _savedInterval = _interval;
        }

        private void RestoreInterval()
        {
            if (_savedInterval == null)
                return;
            _interval = (int)_savedInterval;
            _savedInterval = null;
        }

        #endregion
    }
}
