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

        private int interval;

        private readonly Simulation simulation;

        #endregion

        #region Properties

        public SimulationData SimulationData { get; private set; } = null!;
        public IDataAccess DataAccess { private get => dataAccess; init => dataAccess = value; }
        public ITaskDistributor TaskDistributor { private get => taskDistributor; init => taskDistributor = value; }
        public IController Controller { private get => controller; init => controller = value; }
        public IExecutor Executor { private get => executor; init => executor = value; }
        public ILogger Logger { private get => logger; init => logger = value; }

        public SimulationState SimulationState { get; private set; } = new SimulationState();

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

            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("Robotok"));
            dataAccess = new ConfigDataAccess(path + "sample_files\\simple_test_config.json");

            SimulationData = dataAccess.GetInitialSimulationData();

            controller = new SimpleController();
            taskDistributor = new DemoDistributor(SimulationData);
            executor = new DefaultExecutor(SimulationData);
            Controller.InitializeController(SimulationData, TimeSpan.FromSeconds(6), taskDistributor);

        }

        #region Public methods

        public void StartSimulation()
        {
            if (SimulationState.IsSimulationRunning)
                return;

            if (SimulationState.IsSimulationPaused)
            {
                SimulationState.IsSimulationRunning = true;
                Timer.Start();
                return;
            }

            SetInitialState();

            SimulationState.IsSimulationRunning = true;
            SimulationState.IsSimulationEnded = false;

            Controller.FinishedTask += new EventHandler<IControllerEventArgs>((sender, e) =>
            {
                if (Controller != sender)
                    return;
                OnTaskFinished(e);
            });

            Controller.InitializeController(SimulationData, TimeSpan.FromSeconds(6), taskDistributor);

            Timer.Start();
        }

        public void StopSimulation()
        {
            if (SimulationState.IsSimulationEnded)
                return;

            SimulationState.IsSimulationRunning = false;
            SimulationState.IsSimulationEnded = true;

            Timer.Stop();
            simulation.OnSimulationFinished();
        }

        public void PauseSimulation()
        {
            if(!SimulationState.IsSimulationRunning) return;
            SimulationState.IsSimulationRunning=false;
            Timer.Stop();
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

        public void SetInitialState()
        {
            Timer.Stop();

            SimulationState = new SimulationState();

            SimulationData = dataAccess.GetInitialSimulationData();
            taskDistributor = taskDistributor.NewInstance(SimulationData);
            controller = controller.NewInstance();
            executor = Executor.NewInstance(SimulationData);

            simulation.OnSimulationLoaded();

        }

        public void LoadSimulation(string filePath)
        {
            dataAccess = dataAccess.NewInstance(filePath);
            SetInitialState();
        }

        #endregion

        #region Private methods

        private void StepSimulation()
        {
            Debug.WriteLine("--SIMULATION STEP--");

            if (SimulationState.IsExecutingMoves)
                return;

            if (!SimulationState.IsLastTaskFinished)
            {
                OnTaskTimeout();
                return;
            }

            SimulationState.IsLastTaskFinished = false;
            Controller.CalculateOperations(TimeSpan.FromMilliseconds(interval));
            
        }

        private void OnTaskTimeout()
        {
            Debug.WriteLine("XXXX TIMEOUT XXXX");
            Executor.Timeout();
        }

        private void OnTaskFinished(IControllerEventArgs e)
        {
            Debug.WriteLine("TASK FINISHED");

            SimulationState.IsExecutingMoves = true;
            SimulationState.IsLastTaskFinished = true;
            
            Executor.ExecuteOperations(e.robotOperations);
            SimulationState.IsExecutingMoves = false;

            simulation.OnRobotsMoved();
        }

        #endregion
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
