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

        #endregion

        #region Events

        /// <summary>
        /// Fire with <see cref="OnRobotsMoved"/>
        /// </summary>
        public event EventHandler<RobotsMovedEventArgs>? RobotsMoved;

        /// <summary>
        /// Fire with <see cref="OnGoalChanged"/>
        /// </summary>
        public event EventHandler<Goal?>? GoalChanged;

        /// <summary>
        /// Fire with <see cref="OnSimulationFinished"/>
        /// </summary>
        public event EventHandler? SimulationFinished;

        /// <summary>
        /// Fire with <see cref="OnSimulationLoaded"/>
        /// </summary>
        public event EventHandler? SimulationLoaded;

        // <summary>
        /// Fire with <see cref="OnSimulationStateChanged"/>
        /// </summary>
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
            _mediator = _serviceLocator.GetSimulationMediator(this, _mediator.MapFileName);
        }

        public void LoadLog(string fileName)
        {
            if (_mediator is not IReplayMediator)
            {
                _mediator.SetInitialPosition(); //heggesztés
                _mediator = _serviceLocator.GetReplayMediator(this, _mediator.MapFileName, fileName);
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

            _mediator.SetInitialPosition(); // heggesztés
            _mediator = _serviceLocator.GetSimulationMediator(this, fileName);
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

        #region Mediator methods

        /// <summary>
        /// Call it when robots moved
        /// </summary>
        public void OnRobotsMoved(RobotsMovedEventArgs args)
        {
            RobotsMoved?.Invoke(SimulationData.Robots, args);
        }

        public void OnSimulationLoaded()
        {
            SimulationLoaded?.Invoke(null, new System.EventArgs());
        }

        /// <summary>
        /// Call it when simulation ended
        /// </summary>
        public void OnSimulationFinished()
        {
            SimulationFinished?.Invoke(null, new System.EventArgs());
        }

        /// <summary>
        /// Call it when new simulation data have been loaded
        /// </summary>
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
