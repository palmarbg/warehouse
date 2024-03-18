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
        /// Task distributor used by the simulation
        /// </summary>
        public ITaskDistributor Distributor { get; }

        /// <summary>
        /// Robot controller used by the simulation
        /// </summary>
        public IController Controller { get; }

        #endregion

        #region Events

        /// <summary>
        /// When robots have been added or removed
        /// </summary>
        public event EventHandler? RobotsChanged;

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

        #endregion

        #region Methods

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
