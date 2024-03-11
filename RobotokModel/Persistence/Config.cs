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
        public int RobotCount { get; set; } // teamSize int
        public List<List<Char>> Map { get; set; } = new List<List<char>>(); // mapFile string
        public int MapWidth {get; set; }
        public int MapHeight { get; set; }

        public Strategy DistributionStrategy { get; set; } // taskAssignmentStrategy string
        public List<Position> RobotPositions { get; set; } = new List<Position>(); // agentFile string
        public List<Position> GoalPositions { get; set; } = new List<Position>(); // taskFile string
        public int RevealedTaskCount { get; set; } // numTasksReveal int




    }
}
