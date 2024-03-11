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
        public List<string> Errors { get; set; } = new List<string>();
        public int RobotCount { get; set; }
        public List<Position> RobotStartingPositions { get; set; } = new List<Position>();
        public int StepCount { get; set; }
        public List<List<Char>> RobotPlannedMoves { get; set; } = new List<List<Char>>();
        public List<List<Char>> RobotExecutedMoves { get; set; } = new List<List<Char>>();
        public List<float> TimerCounts { get; set; } = new List<float>();
        // TODO: Task, illetve a többi dolog implementálása


    }
}
