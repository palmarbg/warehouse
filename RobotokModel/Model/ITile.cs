using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model
{
    public interface ITile
    {
        public bool IsPassable { get; }
    }

    public class EmptyTile : ITile
    {
        public bool IsPassable => true;
    }

    public class Block : ITile
    {
        public bool IsPassable => false;
    }

    public class Robot : ITile
    {
        private static int id = 0;

        public static List<Robot> Robots = new();
        public bool IsPassable => true;
        public Direction Rotation { get; set; }
        public Position CurrentGoal { get; set; }
        public RobotOperation NextOperation { get; set; }
        public Position Position { get; set; }
        public int Id { get; }
        public Robot()
        {
            Id = id;
            Rotation = Direction.Left;
            id++;
            Robots.Add(this);
        }
    }
}
