using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DataTypes;

namespace Model.Tests.DataTypes
{
    [TestClass]
    public class SimulationStateTests
    {
        [TestMethod]
        public void SetStateTest()
        {
            var state = new SimulationState();
            state.State = SimulationStates.ControllerWorking;
            Assert.AreEqual(SimulationStates.ControllerWorking, state.State);
        }

        [TestMethod]
        public void IsSimulationRunningTest()
        {
            var state = new SimulationState { State = SimulationStates.ControllerWorking };
            bool result = state.IsSimulationRunning;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsSimulationRunningTest2()
        {
            var state = new SimulationState { State = SimulationStates.ExecutingMoves };
            bool result = state.IsSimulationRunning;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsSimulationRunningTest3()
        {
            var state = new SimulationState { State = SimulationStates.Waiting };
            bool result = state.IsSimulationRunning;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsSimulationRunningTest4()
        {
            var state = new SimulationState { State = SimulationStates.SimulationEnded };
            bool result = state.IsSimulationRunning;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsSimulationRunningTest5()
        {
            var state = new SimulationState { State = SimulationStates.SimulationPaused };
            bool result = state.IsSimulationRunning;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EventTriggeredTest()
        {
            var state = new SimulationState();
            bool eventTriggered = false;
            state.SimulationStateChanged += (sender, args) => { eventTriggered = true; };
            state.State = SimulationStates.ControllerWorking;
            Assert.IsTrue(eventTriggered);
        }
    }
}

