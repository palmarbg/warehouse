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
        public required string MapFile { get; set; }
        public required string AgentFile { get; set; }
        public required int TeamSize { get; set; }
        public required string TaskFile { get; set; }
        public required int NumTasksReveal { get; set; }
        public required string TaskAssignmentStrategy { get; set; }
    }
}
