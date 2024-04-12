using RobotokModel.Model.Controllers;
using RobotokModel.Model.Distributors;
using RobotokModel.Model.Executors;
using RobotokModel.Model.Interfaces;
using RobotokModel.Model.Mediators;
using RobotokModel.Persistence;
using RobotokModel.Persistence.DataAccesses;
using RobotokModel.Persistence.Interfaces;
using RobotokModel.Persistence.Loggers;
using System.Diagnostics;
using System.Timers;

namespace RobotokModel.Model
{
    public class Simulation : ISimulation
    {
        #region Properties
        public int Interval => Mediator.Interval;
        public SimulationData SimulationData => Mediator.SimulationData;
        public SimulationState State => Mediator.SimulationState;
        public IMediator Mediator { get; private set; } = null!;

        #endregion

        #region Events

        /// <summary>
        /// Fire with <see cref="OnRobotsMoved"/>
        /// </summary>
        public event EventHandler? RobotsMoved;

        /// <summary>
        /// Fire with <see cref="OnGoalsChanged"/>
        /// </summary>
        public event EventHandler? GoalsChanged;

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

        public Simulation()
        {

            Goal.GoalsChanged += new EventHandler((_,_) => OnGoalsChanged());

            Mediator = new ReplayMediator(this);
        }

        #endregion

        #region Mediator methods

        /// <summary>
        /// Call it when robots moved
        /// </summary>
        public void OnRobotsMoved()
        {
            RobotsMoved?.Invoke(SimulationData.Robots, new EventArgs());
        }

        /// <summary>
        /// Call it when tasks are completed or created
        /// </summary>
        public void OnGoalsChanged()
        {
            GoalsChanged?.Invoke(SimulationData.Goals, new EventArgs());
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

    }

}
