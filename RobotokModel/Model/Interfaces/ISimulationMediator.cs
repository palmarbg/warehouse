using RobotokModel.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Interfaces
{
    public interface ISimulationMediator : IMediator
    {
        void SaveSimulation(string filepath);

    }
}
