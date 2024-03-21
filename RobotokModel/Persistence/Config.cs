using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RobotokModel.Persistence
{
    public class Config
    {
        public required string mapFile;
        public required string agentFile;
        public required int teamSize;
        public required string taskFile;
        public required int numTasksReveal;
        public required string taskAssignmentStrategy;
    }
}
