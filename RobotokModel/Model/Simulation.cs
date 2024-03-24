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
        private bool IsExecuting;

        private IDataAccess dataAccess;

        #endregion

        #region Properties

        public SimulationData simulationData { get; private set; }
        public ITaskDistributor Distributor { get; private set; } = null!;
        public IController Controller { get; private set; } = null!;
        public IExecutor Executor { get; private set; } = null!;

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
            IsExecuting = false;

            Goal.GoalsChanged += new EventHandler((_,_) => OnGoalsChanged());

            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("Robotok"));
            dataAccess = new ConfigDataAccess(path + "sample_files\\simple_test_config.json");

            simulationData = dataAccess.GetInitialSimulationData();

            SetController("simple");
            SetTaskDistributor("demo");

            Executor = new DefaultExecutor(simulationData);
            Controller.InitializeController(simulationData, TimeSpan.FromSeconds(6), Distributor);
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
                    Controller = new DemoController();
                    break;
                case "simple":
                    Controller = new SimpleController();
                    break;
                default:
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
                    Distributor = new DemoDistributor(simulationData);
                    break;
                default:
                    Distributor = new DemoDistributor(simulationData);
                    return;
            }
        }

        public void SetInitialPosition()
        {
            Timer.Stop();
            isSimulationRunning = false;
            IsExecuting = false;
            isTaskFinished = true;
            
            simulationData = dataAccess.GetInitialSimulationData();
            SetTaskDistributor(simulationData.DistributorName);
            SetController(Controller.Name);
            OnSimulationLoaded();
        }

        public void LoadSimulation(string filePath)
        {
            dataAccess = dataAccess.NewInstance(filePath);
            SetInitialPosition();
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
            if (isTaskFinished && !IsExecuting)
            {
                isTaskFinished = false;
                //Assume it's an async call!
                 Controller.ClaculateOperations(TimeSpan.FromMilliseconds(Interval));
                return;
            } else
            {
                OnTaskTimeout();
            }
        }

        private void OnTaskTimeout()
        {
            Debug.WriteLine("XXXX TIMEOUT XXXX");
            Executor.Timeout();
        }

        private void OnTaskFinished(IControllerEventArgs e)
        {
            isTaskFinished = true;
            Debug.WriteLine("TASK FINISHED");
            IsExecuting = true;
            Executor.ExecuteOperations(e.robotOperations);
            IsExecuting = false;
            OnRobotsMoved();
        }

        #endregion

        #region Event methods

        /// <summary>
        /// Call it when robots moved
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
            SimulationFinished?.Invoke(null, new EventArgs());
        }

        /// <summary>
        /// Call it when new simulation data have been loaded
        /// </summary>
        private void OnSimulationLoaded()
        {
            SimulationLoaded?.Invoke(null, new EventArgs());

            Controller.InitializeController(simulationData, TimeSpan.FromSeconds(6), Distributor);
            Executor = Executor.NewInstance(simulationData);
        }

        #endregion

    }

}
