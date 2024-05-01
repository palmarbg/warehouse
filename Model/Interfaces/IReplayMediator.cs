namespace Model.Interfaces
{
    public interface IReplayMediator : IMediator
    {
        /// <summary>
        /// Set the position to the next one.
        /// </summary>
        public void StepForward();

        /// <summary>
        /// Set the position to the previous one.
        /// </summary>
        public void StepBackward();

        /// <summary>
        /// Set the replay speed relative to <c>1move/sec</c>.
        /// </summary>
        public void SetSpeed(float speed);

        /// <summary>
        /// Set the position to the n-th step.
        /// </summary>
        /// <param name="step">Index of the step</param>
        public void JumpToStep(int step);

        /// <summary>
        /// Set the position to the last step.
        /// </summary>
        public void JumpToEnd();

        /// <summary>
        /// Load log file to replay.
        /// </summary>
        /// <param name="fileName">Absolute path for the log file</param>
        void LoadLog(string fileName);

    }
}
