namespace Model.Interfaces
{
    public interface IReplayMediator : IMediator
    {

        public void StepForward();
        public void StepBackward();


        /// <summary>
        /// When replaying set the replay speed relative to <c>1 move/sec</c>
        /// </summary>
        public void SetSpeed(float speed);

        /// <summary>
        /// When replaying set the position to the n-th step
        /// </summary>
        /// <param name="step"></param>
        public void JumpToStep(int step);
        public void JumpToEnd();

    }
}
