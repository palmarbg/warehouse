using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence
{
    public class Log
    {
        public List<string> Errors { get; set; } = [];
        public int RobotCount { get; set; }
        public List<Position> RobotStartingPositions { get; set; } = [];
        public int StepCount { get; set; }
        public List<List<Char>> RobotPlannedMoves { get; set; } = [];
        public List<List<Char>> RobotExecutedMoves { get; set; } = [];
        public List<float> TimerCounts { get; set; } = [];
        // TODO: Task, illetve a többi dolog implementálása


    }
}
