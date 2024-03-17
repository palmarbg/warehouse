using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model
{

    public static class Extenstions
    {
        public static void RotateClockWise(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    break;
                case Direction.Up:
                    break;
                case Direction.Right:
                    break;
                case Direction.Down:
                    break;
            }
        }
        public static void RotateCounterClockWise(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    break;
                case Direction.Up:
                    break;
                case Direction.Right:
                    break;
                case Direction.Down:
                    break;
            }
        }
        public static char ToChar(this RobotOperation operation)
        {
            char c = 'W';
            switch (operation)
            {
                case RobotOperation.Forward:
                    c = 'F';
                    break;
                case RobotOperation.Clockwise:
                    c = 'R';
                    break;
                case RobotOperation.CounterClockwise:
                    c = 'C';
                    break;
                case RobotOperation.Backward:
                    c = 'B';
                    break;
                case RobotOperation.Wait:
                    c = 'W';
                    break;
            }
            return c;
        }
        public static RobotOperation ToRobotOperation(this char operationChar)
        {
            //TODO: Prototype 2: Custom Exceptions
            switch (operationChar)
            {
                case 'F':
                    return RobotOperation.Forward;
                case 'W':
                    return RobotOperation.Wait;
                case 'C':
                    return RobotOperation.CounterClockwise;
                case 'R':
                    return RobotOperation.Clockwise;
                case 'B':
                    return RobotOperation.Backward;
                default:
                    throw new Exception();
            }
        }
        public static RobotOperation Reverse(this RobotOperation operation)
        {
            switch (operation)
            {
                case RobotOperation.Forward:
                    return RobotOperation.Backward;
                case RobotOperation.Clockwise:
                    return RobotOperation.CounterClockwise;
                case RobotOperation.CounterClockwise:
                    return RobotOperation.Clockwise;
                case RobotOperation.Backward:
                    return RobotOperation.Forward;
                case RobotOperation.Wait:
                    return RobotOperation.Wait;
            }
            return RobotOperation.Wait;
        }
        public static T GetMatrixItemInPosition<T>(this T[,] matrix, Position position)
        {
            return matrix[position.X, position.Y];
        }
        public static void SetMatrixItemInPosition<T>(this T[,] matrix, Position position, T newItem)
        {
            matrix[position.X, position.Y] = newItem;
        }
    }

}
