namespace Persistence.DataTypes
{
    public interface ITile
    {
        public bool IsPassable { get; }
    }

    /// <summary>
    /// Represents an empty tile.
    /// <para/>
    /// Singleton: use <c>Instance</c>
    /// </summary>
    public sealed class EmptyTile : ITile
    {
        public bool IsPassable => true;

        private static readonly EmptyTile _instance = new();

        static EmptyTile() { }
        private EmptyTile() { }
        public static EmptyTile Instance => _instance;
    }

    /// <summary>
    /// Represents a tile that can't be taken by any robot
    /// <para/>
    /// Singleton: use <c>Instance</c>
    /// </summary>
    public sealed class Block : ITile
    {
        public bool IsPassable => false;

        private static readonly Block _instance = new();

        static Block() { }
        private Block() { }
        public static Block Instance => _instance;
    }
}
