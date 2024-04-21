using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Mediators.ReplayMediatorUtils
{
    public interface IReplayController : IController
    {
        void CalculateBackward();
        void SetPosition(int step);
        void JumpToEnd();
    }
}
