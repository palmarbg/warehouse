using Model.DataTypes;
using Model.Interfaces;
using System.Diagnostics;
using System.Timers;

namespace Model.Mediators
{
    public class SimulationMediator : AbstractMediator, ISimulationMediator
    {
        #region Constructor

        public SimulationMediator(ISimulation simulation, IServiceLocator serviceLocator, string mapFileName) : base(simulation, serviceLocator, mapFileName)
        {

            Timer.Elapsed += (_, _) => OnTimerInterval();

            _dataAccess = serviceLocator.GetConfigDataAccess(mapFileName);

            _simulationData = _dataAccess.GetInitialSimulationData();

            _controller = serviceLocator.GetController();

            _executor = _serviceLocator.GetExecutor(_simulationData);

        }

        #endregion

        #region Public methods

        public void SetOptions(int interval, int lastStep)
        {
            _interval = interval;
            _lastStep = lastStep;

            Timer.Interval = _interval;
        }

        public void LoadConfig(string fileName)
        {
            MapFileName = fileName;
            _dataAccess = _dataAccess.NewInstance(fileName);
            SetInitialPosition();
        }

        public void SaveSimulation(string filepath)
        {
            _executor.SaveSimulation(filepath);
        }

        #endregion

        #region Private methods

        private void OnTimerInterval()
        {
            Debug.WriteLine("--SIMULATION STEP--");
            if (_simulationData.Step >= _lastStep)
            {
                StopSimulation();
                return;
            }

            ///The state of the timer defines the state IsSimulationRunning
            ///The timer should be disabled
            if (!_simulationState.IsSimulationRunning)
                throw new SimulationStateException();

            if (_simulationState.State == SimulationStates.ControllerWorking)
            {
                OnTaskTimeout();
                return;
            }

            if (_simulationState.State != SimulationStates.Waiting)
                return;

            _simulationState.State = SimulationStates.ControllerWorking;
            _timeBeforeController = DateTime.Now;
            _controller.CalculateOperations(TimeSpan.FromMilliseconds(_interval));

        }

        private void OnTaskTimeout()
        {
            Debug.WriteLine("XXXX TIMEOUT XXXX");
            _executor.Timeout();
            _simulationData.Step++;
        }

        #endregion
    }
}
