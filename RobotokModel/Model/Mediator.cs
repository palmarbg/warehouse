using RobotokModel.Model.Controllers;
using RobotokModel.Model.Distributors;
using RobotokModel.Model.Executors;
using RobotokModel.Model.Interfaces;
using RobotokModel.Persistence.DataAccesses;
using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RobotokModel.Model
{
    internal class Mediator : IMediator
    {
        #region Private Fields

        private readonly System.Timers.Timer Timer;

        private IDataAccess dataAccess = null!;
        private ITaskDistributor taskDistributor = null!;
        private IController controller = null!;
        private IExecutor executor = null!;
        private ILogger logger = null!;

        private bool isSimulationRunning;
        private bool isTaskFinished;
        private bool IsExecuting;
        private int interval;

        private Simulation simulation;

        #endregion

        #region Properties

        public SimulationData SimulationData { get; private set; } = null!;
        public IDataAccess DataAccess { private get => dataAccess; init => dataAccess = value; }
        public ITaskDistributor TaskDistributor { private get => taskDistributor; init => taskDistributor = value; }
        public IController Controller { private get => controller; init => controller = value; }
        public IExecutor Executor { private get => executor; init => executor = value; }
        public ILogger Logger { private get => logger; init => logger = value; }

        #endregion

        public Mediator(Simulation simulation)
        {
            this.simulation = simulation;

            interval = 1000;
            Timer = new System.Timers.Timer
            {
                Interval = interval,
                Enabled = true
            };
            Timer.Elapsed += (_,_) => StepSimulation();
            Timer.Stop();

            isSimulationRunning = false;
            isTaskFinished = true;
            IsExecuting = false;

            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("Robotok"));
            dataAccess = new ConfigDataAccess(path + "sample_files\\simple_test_config.json");

            SimulationData = dataAccess.GetInitialSimulationData();

            SetController("simple");
            SetController("demo");

            taskDistributor = new DemoDistributor(SimulationData);
            executor = new DefaultExecutor(SimulationData);
            Controller.InitializeController(SimulationData, TimeSpan.FromSeconds(6), taskDistributor);

        }

        #region Public methods

        public void StartSimulation()
        {
            if (isSimulationRunning) return;
            isSimulationRunning = true;
            Timer.Start();

            //to remove
            foreach (Robot robot in SimulationData.Robots)
            {
                taskDistributor.AssignNewTask(robot);
            }
            simulation.OnGoalsChanged();
        }

        public void StopSimulation()
        {
            if (!isSimulationRunning)
                return;

            isSimulationRunning = false;
            Timer.Stop();
            simulation.OnSimulationFinished();
        }

        public void SetController(string name)
        {
            
            switch (name)
            {
                case "demo":
                    controller = new DemoController();
                    break;
                case "simple":
                    controller = new SimpleController();
                    break;
                default:
                    controller = new DemoController();
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
                    taskDistributor = new DemoDistributor(SimulationData);
                    break;
                default:
                    taskDistributor = new DemoDistributor(SimulationData);
                    return;
            }
        }

        public void SetInitialPosition()
        {
            Timer.Stop();
            isSimulationRunning = false;
            IsExecuting = false;
            isTaskFinished = true;

            SimulationData = dataAccess.GetInitialSimulationData();
            SetTaskDistributor(SimulationData.DistributorName);
            SetController(Controller.Name);
            simulation.OnSimulationLoaded();

            Controller.InitializeController(SimulationData, TimeSpan.FromSeconds(6), taskDistributor);
            executor = Executor.NewInstance(SimulationData);
        }

        public void LoadSimulation(string filePath)
        {
            dataAccess = dataAccess.NewInstance(filePath);
            SetInitialPosition();
        }

        #endregion

        #region Private methods

        private void StepSimulation()
        {
            Debug.WriteLine("--SIMULATION STEP--");
            if (Controller == null)
            {
                throw new Exception();
            }
            if (isTaskFinished && !IsExecuting)
            {
                isTaskFinished = false;
                Controller.CalculateOperations(TimeSpan.FromMilliseconds(interval));
                return;
            }
            else
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

            simulation.OnRobotsMoved();
        }

        #endregion


        /*
        public IMediator NewInstance()
        {
            throw new NotImplementedException();
            
            return new Mediator(simulation) {
                DataAccess = DataAccess.NewInstance(),
                TaskDistributor = TaskDistributor.NewInstance(),
                Controller = Controller.NewInstance(),
                Executor = Executor.NewInstance(),
                Logger = Logger.NewInstance()
            };
            
        }
        */
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
