using Model.DataTypes;
using Model.Interfaces;
using Model.Mediators;
using Persistence.DataTypes;
using System.Diagnostics;

namespace Model
{
    public class Simulation : ISimulation
    {
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


        #endregion

        #region Constructor

        public Simulation(IServiceLocator serviceLocator)
        {

            Robot.TaskAssigned += new EventHandler<Goal?>((robot, goal) => OnGoalChanged((Robot)robot!, goal));
            Robot.TaskFinished += new EventHandler<Goal?>((robot, goal) => OnGoalChanged((Robot)robot!, goal));

            Mediator = serviceLocator.GetReplayMediator(this);
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
        public void OnSimulationLoaded()
        {
            SimulationLoaded?.Invoke(null, new EventArgs());
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
