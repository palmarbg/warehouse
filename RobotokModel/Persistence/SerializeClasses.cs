using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence
{
    public class ConfigS
    {
        public string mapFile = "./maps";
        public string agentFile = "./agents";
        public int teamSize = 5;
        public string? taskFile = "./tasks";
        public int numTasksReveal = 1;
        public string taskAssignmentStrategy = "roundrobin";
}

}
