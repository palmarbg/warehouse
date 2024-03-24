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
        public string ActionModel { get; set; } = null!;
        public string AllValid { get; set; } = null!;
        public int TeamSize { get; set; }
        public List<List<Object>> Start { get; set; } = null!;
        public int NumTaskFinished { get; set; }
        public int SumOfCost { get; set; }
        public int MakeSpan { get; set; }
        public List<string> ActualPaths { get; set; } = null!;
        public List<string> PlannerPaths { get; set; } = null!;
        public List<float> PlannerTimes { get; set; } = null!;
        public List<Object> Errors { get; set; } = null!;
        public List<List<List<Object>>> Events { get; set; } = null!;
        public List<List<int>> Tasks { get; set; } = null!;

    }
}
