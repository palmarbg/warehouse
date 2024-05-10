using Persistence.DataTypes;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Model.Executors
{
    public abstract class ExecutorBase(SimulationData simulationData, ILogger logger)
    {
        protected readonly SimulationData _simulationData = simulationData;
        protected readonly ILogger _logger = logger;
        protected List<OperationError> _errors = new List<OperationError>();

        protected void OnTaskFinished(int taskId, int robotId)
        {
            _logger.LogEvent(new(taskId, _simulationData.Step, TaskEventType.finished), robotId);
        }

        protected void OnWallHit(int robotId)
        {
            _errors.Add(new(robotId, -1, _simulationData.Step, OperationErrorType.wallhit));
        }

        protected void OnTimeout()
        {
            _logger.LogTimeout();
            //execute operations deletes it??
            //_errors.Add(new(-1, -1, _simulationData.Step, OperationErrorType.timeout));
        }

        protected void OnRobotCrash(int robotId1, int robotId2)
        {
            _errors.Add(new(robotId1, robotId2, _simulationData.Step, OperationErrorType.collision));
        }

        protected void OnStepFinished(
            RobotOperation[] controllerOperations,
            RobotOperation[] robotOperations,
            OperationError[] errors,
            float timeElapsed
        )
        {
            _logger.LogStep(
                controllerOperations,
                robotOperations,
                errors,
                timeElapsed
            );
        }

        protected void OnTaskAssigned(int taskId, int robotId)
        {
            _logger.LogEvent(new(taskId, _simulationData.Step, TaskEventType.assigned), robotId);
        }
    }
}
