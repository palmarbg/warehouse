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

        #endregion

        #region Properties
        public SimulationData SimulationData => Mediator.SimulationData;
        public SimulationState State => Mediator.SimulationState;
        public IMediator Mediator { get; private set; } = null!;

        #endregion

        #region Events

        /// <summary>
        /// Fire with <see cref="OnRobotsMoved"/>
        /// </summary>
        public event EventHandler<TimeSpan>? RobotsMoved;

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

            Mediator = serviceLocator.GetSimulationMediator(
                this,
                path + "sample_files\\random_20_config.json"//warehouse_100_config
                //, path + "sample_files\\random_20_log.json"//warehouse_100_log
                );
        }

        #endregion

        #region Public methods

        public void LoadConfig(string fileName)
        {
            Mediator.SetInitialState();
            if (Mediator is ISimulationMediator simulationMediator)
            {
                simulationMediator.LoadConfig(fileName);
                return;
            }

            Mediator = _serviceLocator.GetSimulationMediator(this, fileName);
            OnSimulationLoaded();
        }

        public void LoadLog(string fileName)
        {
            Mediator.SetInitialState();
            if (Mediator is not IReplayMediator){
                Mediator = _serviceLocator.GetReplayMediator(this, Mediator.MapFileName, fileName);
                return;
            }
            var mediator = Mediator as IReplayMediator;
            mediator?.LoadLog(fileName);
        }

        public void StartNewSimulation()
        {
            if(Mediator is ISimulationMediator)
            {
                Mediator.SetInitialState();
                return;
            }
            var mediator = Mediator as IReplayMediator;
            Mediator = _serviceLocator.GetSimulationMediator(this, Mediator.MapFileName);

        }

        public void SaveLog(string fileName)
        {
            if (Mediator is not ISimulationMediator)
                return;
            var mediator = Mediator as ISimulationMediator;
            mediator?.SaveSimulation(fileName);
        }

        #endregion

        #region Mediator methods

        /// <summary>
        /// Call it when robots moved
        /// </summary>
        public void OnRobotsMoved(TimeSpan timeSpan)
        {
            RobotsMoved?.Invoke(SimulationData.Robots, timeSpan);
        }

        public void OnSimulationLoaded()
        {
            SimulationLoaded?.Invoke(null, new EventArgs());
        }

        /// <summary>
        /// Call it when simulation ended
        /// </summary>
        public void OnSimulationFinished()
        {
            SimulationFinished?.Invoke(null, new EventArgs());
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
                    IsReplayMode = Mediator is IReplayMediator
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
