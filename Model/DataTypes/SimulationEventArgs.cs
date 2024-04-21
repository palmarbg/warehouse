using Persistence.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataTypes
{
    public class GoalChanged : EventArgs
    {
        private readonly Robot _robot;
        private readonly Goal _goal;

        public Robot Robot { get; set; }
        public Goal Goal { get; set; }

        public GoalChanged(Robot robot, Goal goal)
        {
            _robot = robot;
            _goal = goal;
        }
    }
}
