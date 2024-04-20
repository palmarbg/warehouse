namespace Persistence.DataTypes
{
    public class Robot : ITile
    {
        public bool IsPassable => false;

        public required int Id { get; init; }
        public required Position Position { get; set; }
        public required Direction Rotation { get; set; }
        public RobotOperation NextOperation { get; set; }

        /// <summary>
        /// null if there is no current goal
        /// </summary>
        public Goal? CurrentGoal { get; set; }

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
