using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Persistence.Interfaces
{
    /// <summary>
    /// Config file access
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Returns the starting position loaded by <see cref="LoadAsync"/>
        /// </summary>
        SimulationData SimulationData { get; }

        /// <summary>
        /// Load config file
        /// </summary>
        /// <param name="path">Absolute path for config file</param>
        /// <returns></returns>
        //Task LoadAsync(string path); //alternative: void Load(string path)
        void Load(string path);
    }
}
