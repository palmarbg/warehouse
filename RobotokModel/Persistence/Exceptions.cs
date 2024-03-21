using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence
{
    public class JSonError : Exception
    {
        public JSonError(string message) : base(message) { }
        public JSonError() : base() { }
    }
}
