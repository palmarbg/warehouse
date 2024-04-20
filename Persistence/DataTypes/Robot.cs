namespace Persistence.DataTypes
{
    public class Robot : ITile
    {
        #region Backup Fields

        private Goal? _goal = null;

        #endregion

        #region Static

        /// <summary>
        /// Fired when a robots goal is changed
        /// </summary>
        public static event EventHandler<Goal>? TaskAssigned;
        public static event EventHandler<Goal>? TaskFinished;

        private static void OnTaskAssigned(Robot robot, Goal goal)
        {
            TaskAssigned?.Invoke(robot, goal);
        }

        private static void OnTaskFinished(Robot robot, Goal goal)
        {
            TaskFinished?.Invoke(robot, goal);
        }

        #endregion

        public bool IsPassable => false;

        public required int Id { get; init; }
        public required Position Position { get; set; }
        public required Direction Rotation { get; set; }
        public RobotOperation NextOperation { get; set; }

        /// <summary>
        /// null if there is no current goal
        /// </summary>
        public Goal? CurrentGoal
        {
            get => _goal;
            set
            {
                if(value == null && _goal != null)
                {
                    _goal.IsAssigned = false;
                    OnTaskFinished(this, _goal);
                }
                if (value != null)
                {
                    value.IsAssigned = true;
                    OnTaskAssigned(this, value);
                }
                _goal = value;
            }
        }

        public bool MovedThisTurn { get; set; } = false;
        public bool InspectedThisTurn { get; set; } = false;
        public bool BlockedThisTurn { get; set; } = false;

        // For Debug
        //public override string ToString()
        //{
        //    return Position.ToString();
        //}
    }
}
