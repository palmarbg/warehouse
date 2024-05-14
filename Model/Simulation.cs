using Model.DataTypes;
using Model.Interfaces;
using Persistence.DataTypes;
using System.Diagnostics;

namespace Model
{
    public class Simulation : ISimulation
    {
        #region Private Fields

        private IServiceLocator _serviceLocator;
        private IMediator _mediator;

        #endregion

        #region Private Properties
        private IReplayMediator ReplayMediator
        {
            get
            {
                if (_mediator is not IReplayMediator)
                    throw new SimulationStateException();

                return (IReplayMediator)_mediator!;
            }
        }

        private ISimulationMediator SimulationMediator
        {
            get
            {
                if (_mediator is not ISimulationMediator)
                    throw new SimulationStateException();

                return (ISimulationMediator)_mediator!;
            }
        }

        #endregion

        #region Public Properties

        public SimulationData SimulationData => _mediator.SimulationData;
        public SimulationState SimulationState => _mediator.SimulationState;
        public int Interval => _mediator.Interval;
        public string MapFileName => _mediator.MapFileName;

        public bool IsInSimulationMode => _mediator is ISimulationMediator;

        public int SimulationStepLimit => _mediator.SimulationStepLimit;


        #endregion

        #region Events

        public event EventHandler<RobotsMovedEventArgs>? RobotsMoved;

        public event EventHandler<Goal?>? GoalChanged;

        public event EventHandler<SimulationStepEventArgs>? SimulationStep;

        public event EventHandler? SimulationLoaded;

        public event EventHandler<SimulationStateEventArgs>? SimulationStateChanged;

        #endregion

        #region Constructor

        public Simulation(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            Robot.TaskAssigned += new EventHandler<Goal?>((robot, goal) => OnGoalChanged((Robot)robot!, goal));
            Robot.TaskFinished += new EventHandler<Goal?>((robot, goal) => OnGoalChanged((Robot)robot!, goal));

            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("View"));

            _mediator = serviceLocator.GetSimulationMediator(
                this,
                path + "sample_files\\random_20_config.json"//warehouse_100_config
                                                            //, path + "sample_files\\random_20_log.json"//warehouse_100_log
                );
        }

        #endregion

        #region Public StateChange methods

        public void StartNewSimulation()
        {
            if (_mediator is ISimulationMediator)
            {
                _mediator.SetInitialPosition();
                return;
            }
            _mediator.Dispose();
            _mediator = _serviceLocator.GetSimulationMediator(this, _mediator.MapFileName);
            OnSimulationLoaded();
        }

        public void LoadLog(string fileName)
        {
            if (_mediator is not IReplayMediator)
            {
                _mediator.Dispose();
                _mediator = _serviceLocator.GetReplayMediator(this, _mediator.MapFileName, fileName);
                OnSimulationLoaded();
                return;
            }
            ReplayMediator.LoadLog(fileName);
        }

        public void LoadConfig(string fileName)
        {
            if (_mediator is ISimulationMediator simulationMediator)
            {
                simulationMediator.LoadConfig(fileName);
                return;
            }
            _mediator.Dispose();
            _mediator = _serviceLocator.GetSimulationMediator(this, fileName);
            OnSimulationLoaded();
        }

        #endregion

        #region Public Mediator methods

        public void StartSimulation()
        {
            _mediator.StartSimulation();
        }

        public void StopSimulation()
        {
            _mediator.StopSimulation();
        }

        public void PauseSimulation()
        {
            _mediator.PauseSimulation();
        }

        public void SetInitialPosition()
        {
            _mediator.SetInitialPosition();
        }

        #endregion

        #region Public Replay methods

        public void StepForward()
        {
            ReplayMediator.StepForward();
        }

        public void StepBackward()
        {
            ReplayMediator.StepBackward();
        }

        public void SetSpeed(float speed)
        {
            ReplayMediator.SetSpeed(speed);
        }

        public void JumpToStep(int step)
        {
            ReplayMediator.JumpToStep(step);
        }

        public void JumpToEnd()
        {
            ReplayMediator.JumpToEnd();
        }

        #endregion

        #region Public Simulation methods

        public void SaveSimulation(string filepath)
        {
            SimulationMediator.SaveSimulation(filepath);
        }
        public void SetOptions(int interval, int lastStep)
        {
            SimulationMediator.SetOptions(interval, lastStep);
        }

        #endregion

        #region Dispose

        /// <summary>
        /// The cleanup after the simulation is not implemented, does not hold any special resource.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Mediator methods

        public void OnRobotsMoved(RobotsMovedEventArgs args)
        {
            RobotsMoved?.Invoke(SimulationData.Robots, args);
        }

        public void OnSimulationLoaded()
        {
            SimulationLoaded?.Invoke(null, new System.EventArgs());
        }

        public void OnSimulationStep()
        {
            SimulationStep?.Invoke(null, new(SimulationData.Step));
        }

        public void OnSimulationStateChanged(SimulationState simulationState)
        {
            SimulationStateChanged?.Invoke(
                null,
                new SimulationStateEventArgs
                {
                    SimulationState = simulationState,
                    IsReplayMode = _mediator is IReplayMediator
                }
                );
        }

        #endregion

        #region Private methods

        private void OnGoalChanged(Robot robot, Goal? goal)
        {
            GoalChanged?.Invoke(robot, goal);
        }

        #endregion

    }

}
