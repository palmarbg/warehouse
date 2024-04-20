namespace Persistence.DataTypes
{
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
