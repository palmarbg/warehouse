using Model.Interfaces;
using Persistence.DataTypes;

namespace Model.Utils.ReplayMediatorUtils
{
    /// <summary>
    /// Provides additional features for <see cref="IReplayMediator"/>.
    /// </summary>
    public interface IReplayController : IController
    {
        /// <summary>
        /// Calculates the moves to take one step backward.
        /// </summary>
        void CalculateBackward();

        /// <summary>
        /// Jumps to the <paramref name="step"/>-th step.
        /// </summary>
        /// <param name="step">Index of the simulation step to jump.</param>
        void SetPosition(int step);

        /// <summary>
        /// Jumps to the last simulation step.
        /// </summary>
        void JumpToEnd();

        /// <summary>
        /// Returns how many steps does the simulation take.
        /// </summary>
        int GetSimulationLength();
    }
}
