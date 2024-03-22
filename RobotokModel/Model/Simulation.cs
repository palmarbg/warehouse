using RobotokModel.Model.Controllers;
using RobotokModel.Model.Distributors;
using RobotokModel.Model.Executors;
using RobotokModel.Model.Interfaces;
using RobotokModel.Persistence;
using RobotokModel.Persistence.DataAccesses;
using RobotokModel.Persistence.Interfaces;
using System.Diagnostics;
using System.Timers;

namespace RobotokModel.Model
{
    public class Simulation : ISimulation
    {

        #region Fields

        private readonly System.Timers.Timer Timer;
        private bool isSimulationRunning;
        private bool isTaskFinished;

        #endregion

        #region Properties

        public SimulationData simulationData { get; private set; }
        public ITaskDistributor? Distributor { get; private set; }
        public IController? Controller { get; private set; }

        public IExecutor? Executor { get; private set; }

        /// <summary>
        /// Timespan that Controller has to finish task
        /// </summary>
        public int Interval { get; private set; }

        //not used
        //private List<List<RobotOperation>> ExecutedOperations { get; set; } = [];
        //private Log CurrentLog { get; set; }
        //private int RemainingSteps { get; set; }

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

        public Simulation()
        {
            Interval = 1000;
            Timer = new System.Timers.Timer
            {
                Interval = Interval,
                Enabled = true
            };
            Timer.Elapsed += StepSimulation;
            Timer.Stop();

            isSimulationRunning = false;
            isTaskFinished = true;

            Goal.GoalsChanged += new EventHandler((_,_) => OnGoalsChanged());

            IDataAccess dataAccess = new DemoDataAccess();
            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("Robotok"));
            dataAccess.Load(path + "sample_files\\random_20_config.json");

            simulationData = dataAccess.SimulationData;

            SetController("demo");
            SetTaskDistributor("demo");
            Executor = new DefaultExecutor(simulationData);

            Controller?.InitializeController(simulationData, TimeSpan.FromSeconds(5));

        }

        #endregion

        #region Public methods
        
        public void StartSimulation()
        {
            if(isSimulationRunning) return;
            isSimulationRunning = true;
            Timer.Start();

            //to remove
            foreach(Robot robot in simulationData.Robots)
            {
                Distributor?.AssignNewTask(robot);
            }
            OnGoalsChanged();
        }

        public void StopSimulation()
        {
            if (!isSimulationRunning)
                return;

            isSimulationRunning = false;
            Timer.Stop();
            OnSimulationFinished();
        }

        public void SetController(string name)
        {
            //switch case might be refactored into something else
            switch (name)
            {
                case "demo":
                default :
                    Controller = new DemoController();
                    break;
            }
            Controller.FinishedTask += new EventHandler<IControllerEventArgs>((sender, e) =>
            {
                if (Controller != sender)
                    return;
                OnTaskFinished(e);
            });
        }

        public void SetTaskDistributor(string name)
        {
            switch (name)
            {
                case "demo":
                default :
                    Distributor = new DemoDistributor(simulationData);
                    return;
            }
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

        private void StepSimulation(object? sender, ElapsedEventArgs e)
        {
            Debug.WriteLine("--SIMULATION STEP--");
            if (Controller == null)
            {
                throw new Exception();
            }
            if (isTaskFinished)
            {
                isTaskFinished = false;
                //Assume it's an async call!
                Controller?.ClaculateOperations(TimeSpan.FromMilliseconds(Interval));
                return;
            } else
            {
                OnTaskTimeout();
            }
        }

        private void OnTaskTimeout()
        {
            Debug.WriteLine("XXXX TIMEOUT XXXX");
            Executor?.Timeout();
        }

        private void OnTaskFinished(IControllerEventArgs e)
        {
            isTaskFinished = true;
            Debug.WriteLine("TASK FINISHED");
            Executor?.ExecuteOperations(e.robotOperations);
            OnRobotsMoved();
        }


        /*
        // TODO: Prototype 2 : Log planned and executed moves
        private void SimulationStep(object? sender, ElapsedEventArgs args)
        {
            var operations = ((PrimitiveController)Controller).NextStep();
            for (int i = 0; i < simulationData.Robots.Length; i++)
            {
                var robot = simulationData.Robots[i];
                if (!robot.MovedThisTurn) robot.MoveRobot(this);
            }
            Robot.EndTurn();
            OnRobotsMoved();
        }
        */

        #endregion

        #region Event methods

        /// <summary>
        /// Call when the robot collection changed
        /// </summary>
        private void OnRobotsChanged()
        {
            RobotsChanged?.Invoke(simulationData.Robots, new EventArgs());
        }

        /// <summary>
        /// Call it when the robots moved, but the collection didn't change
        /// <para />
        /// If the collection changed call <see cref="OnRobotsChanged"/>
        /// </summary>
        private void OnRobotsMoved()
        {
            RobotsMoved?.Invoke(simulationData.Robots, new EventArgs());
        }

        /// <summary>
        /// Call it when tasks are completed or created
        /// </summary>
        private void OnGoalsChanged()
        {
            GoalsChanged?.Invoke(simulationData.Goals, new EventArgs());
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
