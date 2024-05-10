using Model.Interfaces;
using Persistence.DataTypes;

namespace Model.Mediators.ReplayMediatorUtils
{
    public interface IReplayController : IController
    {
        void CalculateBackward();
        RobotOperation[] SetPosition(int step);
        RobotOperation[] JumpToEnd();

        int GetSimulationLength();
    }
}
