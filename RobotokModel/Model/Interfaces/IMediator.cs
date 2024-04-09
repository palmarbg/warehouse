using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Interfaces
{
    public interface IMediator
    {
        #region Properties

        public SimulationData SimulationData { get; }
        public SimulationState SimulationState { get; }
        public IDataAccess DataAccess { init; }
        public ITaskDistributor TaskDistributor { init; }
        public IController Controller { init; }
        public IExecutor Executor { init; }
        public ILogger Logger { init; }

        #endregion

        #region Methods

        void StartSimulation();
        void StopSimulation();
        void PauseSimulation();
        void SetController(string name);
        void SetTaskDistributor(string name);
        void SetInitialState();

        /// <summary>
        /// Start new simulation from config file
        /// </summary>
        /// <param name="filePath">Absolute path for config file</param>
        void LoadSimulation(string filePath);

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


        public Log GetLog();

        */

        #endregion

    }
}
