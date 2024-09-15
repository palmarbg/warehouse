using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataTypes
{
    class SimulationStateException : Exception
    {
        public SimulationStateException()
        {
        }
    }

    class SimulationStepOutOfRangeException : Exception
    {
        public SimulationStepOutOfRangeException()
        {
        }
    }
}
