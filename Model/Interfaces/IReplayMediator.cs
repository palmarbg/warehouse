namespace Model.Interfaces
{
    public interface IReplayMediator : IMediator
    {
        /// <summary>
        /// Takes the simulation one step forward.
        /// </summary>
        public void StepForward();

        /// <summary>
        /// Takes the simulation one step backward.
        /// </summary>
        public void StepBackward();

        /// <summary>
        /// Sets the replay speed relative to <c>1move/sec</c>.
        /// </summary>
        public void SetSpeed(float speed);

        /// <summary>
        /// Sets the position to the n-th step.
        /// </summary>
        /// <param name="step">Index of the step.</param>
        public void JumpToStep(int step);

        /// <summary>
        /// Sets the position to the last step.
        /// </summary>
        public void JumpToEnd();

        /// <summary>
        /// Loads the log file to replay.
        /// </summary>
        /// <param name="fileName">Absolute path of the log file.</param>
        void LoadLog(string fileName);

    }
}
