using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Extensions
{
    public static class RobotOperationExtensions
    {
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
            return operationChar switch
            {
                'F' => RobotOperation.Forward,
                'W' => RobotOperation.Wait,
                'C' => RobotOperation.CounterClockwise,
                'R' => RobotOperation.Clockwise,
                'B' => RobotOperation.Backward,
                _ => throw new Exception(),
            };
        }

        public static RobotOperation Reverse(this RobotOperation operation)
        {
            return operation switch
            {
                RobotOperation.Forward => RobotOperation.Backward,
                RobotOperation.Clockwise => RobotOperation.CounterClockwise,
                RobotOperation.CounterClockwise => RobotOperation.Clockwise,
                RobotOperation.Backward => RobotOperation.Forward,
                RobotOperation.Wait => RobotOperation.Wait,
                _ => RobotOperation.Wait,
            };
        }
    }
}
