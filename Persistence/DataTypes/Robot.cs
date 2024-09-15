namespace Persistence.DataTypes
{
    public class Robot : ITile
    {
        #region Backup Fields

        private Goal? _goal = null;

        #endregion

        #region Static

        /// <summary>
        /// Occurs when a robot gets a new task assigned
        /// </summary>
        public static event EventHandler<Goal?>? TaskAssigned;

        /// <summary>
        /// Occurs when a robot finished it's task
        /// </summary>
        public static event EventHandler<Goal?>? TaskFinished;

        private static void OnTaskAssigned(Robot robot, Goal? goal)
        {
            TaskAssigned?.Invoke(robot, goal);
        }

        private static void OnTaskFinished(Robot robot, Goal? goal)
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
                if (value == null && _goal == null)
                    return;
                if (value == null && _goal != null)
                {
                    _goal.IsAssigned = false;
                    OnTaskFinished(this, null);
                    _goal = null;

                }
                else
                if (value != null && _goal == null)
                {
                    value.IsAssigned = true;
                    _goal = value;
                    OnTaskAssigned(this, _goal);
                }
                else
                if (value != null && _goal != null)
                {
                    _goal.IsAssigned = false;
                    value.IsAssigned = true;
                    _goal = value;
                    OnTaskAssigned(this, _goal);
                }
            }
        }

        public bool MovedThisTurn { get; set; } = false;
        public bool InspectedThisTurn { get; set; } = false;
        public bool BlockedThisTurn { get; set; } = false;

    }
}
