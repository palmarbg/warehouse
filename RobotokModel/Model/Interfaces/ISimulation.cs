using RobotokModel.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Interfaces
{
    public interface ISimulation
    {

        #region Properties

        /// <summary>
        /// Contains the current state of the map
        /// </summary>
        public SimulationData SimulationData { get; }

        /// <summary>
        /// Mediator used by the simulation
        /// </summary>
        public IMediator Mediator { get; }

        public SimulationState State => Mediator.SimulationState;

        #endregion

        #region Events

        /// <summary>
        /// When robots moved
        /// </summary>
        public event EventHandler? RobotsMoved;

        /// <summary>
        /// When goals have been added or completed
        /// </summary>
        public event EventHandler? GoalsChanged;

        /// <summary>
        /// When simulation stops
        /// </summary>
        public event EventHandler? SimulationFinished;

        /// <summary>
        /// When map have been loaded
        /// </summary>
        public event EventHandler? SimulationLoaded;

        #endregion

        #region Methods
        /*
        /// <summary>
        /// Sets the controller used by the simulation
        /// </summary>
        /// <param name="name">Name of the controller</param>
        public void SetController(string name);

        /// <summary>
        /// Sets the task distributor used by the simulation
        /// </summary>
        /// <param name="name">Name of the task distributor</param>
        public void SetTaskDistributor(string name);


        /// <summary>
        /// Start simulation
        /// </summary>
        public void StartSimulation();

        /// <summary>
        /// Stop simulation
        /// </summary>
        public void StopSimulation();

        /// <summary>
        /// Sets the state of the simulation to the initial position loaded from config files
        /// </summary>
        public void SetInitialPosition();

        /// <summary>
        /// Start new simulation from config file
        /// </summary>
        /// <param name="filePath">Absolute path for config file</param>
        public void LoadSimulation(string filePath);
        */
        /*
        /// <summary>
        /// Pause simulation
        /// </summary>
        public void PauseSimulation();

        /// <summary>
        /// ???
        /// </summary>
        public void StepForward();

        /// <summary>
        /// ???
        /// </summary>
        public void StepBackward();

        /// <summary>
        /// When replaying set the replay speed relative to <c>1 move/sec</c>
        /// </summary>
        public void SetSimulationSpeed(double speed);

        /// <summary>
        /// When replaying set the position to the n-th step
        /// </summary>
        /// <param name="step"></param>
        public void JumpToStep(int step);

        /// <summary>
        /// ???
        /// </summary>
        public Log GetLog();

        */

        #endregion

    }
}
