namespace Persistence.DataTypes
{
    /// <summary>
    /// Represents the current state of the warehouse, including robots and goals.
    /// </summary>
    public class SimulationData
    {
        public required ITile[,] Map;
        public required List<Goal> Goals;
        public required List<Robot> Robots;
        public required string DistributorName;
        public string? ControllerName;
        public required int RevealedTaskCount;
        public int Step { get; set; } = 0;
    }
}
