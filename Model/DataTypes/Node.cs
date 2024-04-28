using Persistence.DataTypes;
using Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataTypes
{
    public class Node
    {
        public Position Position { get; set; }
        public Node? parent;
        public Direction? Direction { get; set; } = null;
        public int Turn = 0;
        public int AllowedRotations = 0;
        public Node(Position pos)
        {
            Position = pos;
        }
        public bool SameAs(Node other)
        {
            return Position.Equals(other.Position);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Node node)
                return Position.EqualsPosition(node.Position);
            else
                return base.Equals(obj);
        }
        // https://stackoverflow.com/questions/371328/why-is-it-important-to-override-gethashcode-when-equals-method-is-overridden
        public override int GetHashCode()
        {
            unchecked // overflow működésben nem okoz hibát, de exception dobna
            {
                int hash = 13;
                hash = hash * 3 + Position.X.GetHashCode();
                hash = hash * 7 + Position.Y.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return Position.ToString();
        }

        public static Node NewWaitNode()
        {
            return new Node(new Position() { X = -1, Y = -1 });
        }
    }
}
