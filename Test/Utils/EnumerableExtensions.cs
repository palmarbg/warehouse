using Persistence.DataTypes;
using Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<RobotOperation> AssignToRobots(this IEnumerable<RobotOperation> seq, List<Robot> robots)
        {
            RobotOperation[] arr = seq.ToArray();

            if (arr.Length == 0 || arr.Length > robots.Count || robots.Count % arr.Length != 0)
                throw new ArgumentException("Could not broadcast");

            RobotOperation[] result = new RobotOperation[robots.Count];
            for (int i = 0; i < robots.Count; i++)
                for (int j = 0; j < arr.Length; j++)
                {
                    robots[i].NextOperation = arr[j];
                    result[i] = arr[j];
                    i++;
                }
            return result;
        }

        public static IEnumerable<Direction> AssignToRobots(this IEnumerable<Direction> seq, List<Robot> robots)
        {
            Direction[] arr = seq.ToArray();

            if (arr.Length == 0 || arr.Length > robots.Count || robots.Count % arr.Length != 0)
                throw new ArgumentException("Could not broadcast");

            Direction[] result = new Direction[robots.Count];
            for (int i = 0; i < robots.Count; i++)
                for (int j = 0; j < arr.Length; j++)
                {
                    robots[i].Rotation = arr[j];
                    result[i] = arr[j];
                    i++;
                }
            return result;
        }

        public static IEnumerable<Direction> SetClockwiseRotation(this IEnumerable<Direction> seq)
        {
            var iterator = seq.GetEnumerator();
            Direction last = iterator.Current;
            foreach (var dir in seq)
            {
                var current = last;
                last = last.RotateClockWise();
                yield return current;
            }
        }

        public static IEnumerable<Direction> SetCounterClockwiseRotation(this IEnumerable<Direction> seq)
        {
            var iterator = seq.GetEnumerator();
            Direction last = iterator.Current;
            foreach (var dir in seq)
            {
                var current = last;
                last = last.RotateCounterClockWise();
                yield return current;
            }
        }
    }
}
