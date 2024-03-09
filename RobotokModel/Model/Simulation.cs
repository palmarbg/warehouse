using RobotokModel.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model
{
    public class Simulation
    {
        private SimulationData SimulationData { get; set; }
        private List<List<RobotOperation>> ExecutedOperations { get; set; } = new List<List<RobotOperation>>();
        private Log CurrentLog { get; set; }
        private ITaskDistributor Distributor { get; set; }
        private IController Controller { get; set; }
        private int RemainingSteps { get; set; }


        public Simulation(Config config, string Strategy, int StepLimit )
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
        public void StartSimulation()
        {
            throw new NotImplementedException();
        }
       
        private void SimulationStep()
        {
            
            var operations = Controller.NextStep();
            for (int i = 0; i < Robot.Robots.Count; i++)
            {
                var robot = Robot.Robots[i];
                switch (operations[i])
                {
                    case RobotOperation.Forward:
                        
                        break;
                    case RobotOperation.Clockwise:
                        robot.Rotation.RotateClockWise();
                        break;
                    case RobotOperation.CounterClockwise:
                        robot.Rotation.RotateCounterClockWise();
                        break;
                    case RobotOperation.Backward:
                        throw new NotImplementedException();

                        break;
                    case RobotOperation.Wait:
                        throw new NotImplementedException();

                        break;
                }
            }
            throw new NotImplementedException();

        }
        public void StopSimulation()
        {
            throw new NotImplementedException();
        }
        public void PauseSimulation()
        {
            throw new NotImplementedException();
        }
        public void SetSimulationSpeed()
        {
            throw new NotImplementedException();
        }
        public void JumpToStep(int step)
        {
            throw new NotImplementedException();
        }
        public void GetLog(Log log)
        {
            throw new NotImplementedException();
        }
        private int GoalsRemaining()
        {
            throw new NotImplementedException();
        }
        private RobotOperation ReverseOperation(RobotOperation operation)
        {
            switch (operation)
            {
                case RobotOperation.Forward:
                    return RobotOperation.Backward;
                case RobotOperation.Clockwise:
                    return RobotOperation.CounterClockwise ;
                case RobotOperation.CounterClockwise:
                    return RobotOperation.Clockwise ;
                case RobotOperation.Backward:
                    return RobotOperation.Forward;
                case RobotOperation.Wait:
                    return RobotOperation.Wait;
            }
            return RobotOperation.Wait ;
        }


    }
}
