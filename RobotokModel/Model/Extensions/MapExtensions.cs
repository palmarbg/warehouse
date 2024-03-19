using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Extensions
{
    public static class MapExtensions
    {
        public static T GetAtPosition<T>(this T[,] matrix, Position position)
        {
            return matrix[position.X, position.Y];
        }
        public static void SetAtPosition<T>(this T[,] matrix, Position position, T newItem)
        {
            matrix[position.X, position.Y] = newItem;
        }

    }
}
