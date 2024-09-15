using Persistence.DataTypes;

namespace Persistence.Extensions
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

        public static bool IsOnMap<T>(this T[,] matrix, Position position)
        {
            return
                position.X >= 0 &&
                position.Y >= 0 &&
                position.X < matrix.GetWidth() &&
                position.Y < matrix.GetHeight();
        }

        public static int GetWidth<T>(this T[,] matrix)
        {
            return matrix.GetLength(0);
        }

        public static int GetHeight<T>(this T[,] matrix)
        {
            return matrix.GetLength(1);
        }

        public static (int, int) ToXY<T>(this int n, T[,] matrix)
        {
            return (n % matrix.GetWidth(), n / matrix.GetWidth());
        }

        public static int ToInt<T>(this (int, int) xy, T[,] matrix)
        {
            return (xy.Item2 * matrix.GetWidth() + xy.Item1);
        }

    }
}
