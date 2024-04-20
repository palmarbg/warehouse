namespace Persistence.DataTypes
{
    public class Goal
    {
        #region Static

        public static event EventHandler? GoalsChanged;
        public static void OnGoalsChanged()
        {
            GoalsChanged?.Invoke(null, new());
        }

        #endregion
        public required int Id { get; init; }
        public required Position Position { get; set; }
        public bool IsAssigned { get; set; } = false;

        public Goal() { }

    }
}
