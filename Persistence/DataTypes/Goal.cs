namespace Persistence.DataTypes
{
    public class Goal
    {
        public required int Id { get; init; }
        public required Position Position { get; set; }
        public bool IsAssigned { get; set; } = false;

        public Goal() { }

    }
}
