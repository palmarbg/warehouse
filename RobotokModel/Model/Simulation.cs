using RobotokModel.Model.Interfaces;
using RobotokModel.Persistence;
using System.Timers;

namespace RobotokModel.Model
{
    public class Simulation : ISimulation
    {

        #region Fields

        private readonly System.Timers.Timer Timer;
        private bool SimulationRunning;

        #endregion

        #region Properties

        public SimulationData SimulationData { get; private set; }
        public ITaskDistributor Distributor { get; private set; }
        public IController Controller { get; private set; }


        private List<List<RobotOperation>> ExecutedOperations { get; set; } = [];
        private Log CurrentLog { get; set; }
        private int RemainingSteps { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Fire with <see cref="OnRobotsChanged" />
        /// </summary>
        public event EventHandler? RobotsChanged;

        /// <summary>
        /// Fire with <see cref="OnRobotsMoved" />
        /// </summary>
        public event EventHandler? RobotsMoved;

        /// <summary>
        /// Fire with <see cref="OnGoalsChanged" />
        /// </summary>
        public event EventHandler? GoalsChanged;

        /// <summary>
        /// Fire with <see cref="OnSimulationFinished" />
        /// </summary>
        public event EventHandler? SimulationFinished;


        #endregion

        #region Constructor

        public Simulation(Config config, string Strategy, int StepLimit)
        {

            Timer = new System.Timers.Timer
            {
                Interval = 1000,
                Enabled = true
            };
            Timer.Elapsed += SimulationStep;
            SimulationRunning = false;

            Goal.GoalsChanged += new EventHandler((_,_) => OnGoalsChanged());

            SimulationData = new() {
                Map = new ITile[5,5]
            };
            
            //throw new NotImplementedException();

        }

        #endregion

        #region Public methods
        
        public void StartSimulation()
        {
            SimulationRunning = true;
            Timer.Start();

            throw new NotImplementedException();
        }

        public void StopSimulation()
        {
            if (SimulationRunning)
            {
                SimulationRunning = false;
                Timer.Stop();
                OnSimulationFinished();
            }
            else throw new NotImplementedException();

            throw new NotImplementedException();
        }

        /*
         * These will be implemented later on
        public void PauseSimulation()
        {
            throw new NotImplementedException();
        }
        public void StepForward()
        {
            throw new NotImplementedException();
        }

        public void StepBackward()
        {
            throw new NotImplementedException();
        }
        public void SetSimulationSpeed(double speed)
        {
            throw new NotImplementedException();
        }
        public void JumpToStep(int step)
        {
            throw new NotImplementedException();
        }
        public Log GetLog()
        {
            throw new NotImplementedException();
        }
        */

        #endregion

        #region Private methods

        // TODO: Prototype 2 : Log planned and executed moves
        private void SimulationStep(object? sender, ElapsedEventArgs args)
        {
            var operations = Controller.NextStep();
            for (int i = 0; i < Robot.Robots.Count; i++)
            {
                var robot = Robot.Robots[i];
                if (!robot.MovedThisTurn) robot.MoveRobot(this);
            }
            Robot.EndTurn();
            OnRobotsMoved();
        }

        #endregion

        #region Event methods

        /// <summary>
        /// Call when the robot collection changed
        /// </summary>
        private void OnRobotsChanged()
        {
            RobotsChanged?.Invoke(Robot.Robots, new EventArgs());
        }

        /// <summary>
        /// Call it when the robots moved, but the collection didn't change
        /// <para />
        /// If the collection changed call <see cref="OnRobotsChanged"/>
        /// </summary>
        private void OnRobotsMoved()
        {
            RobotsMoved?.Invoke(Robot.Robots, new EventArgs());
        }

        /// <summary>
        /// Call it when tasks are completed or created
        /// </summary>
        private void OnGoalsChanged()
        {
            GoalsChanged?.Invoke(SimulationData.Goals, new EventArgs());
        }

        /// <summary>
        /// Call it when simulation ended
        /// </summary>
        private void OnSimulationFinished()
        {
            SimulationFinished?.Invoke(this, new EventArgs());
        }

        #endregion

    }

}
