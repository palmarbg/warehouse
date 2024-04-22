using Model.Interfaces;

namespace Model.Mediators.ReplayMediatorUtils
{
    public interface IReplayController : IController
    {
        void CalculateBackward();
        void SetPosition(int step);
        void JumpToEnd();
    }
}
