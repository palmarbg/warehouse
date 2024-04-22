using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Interfaces
{
    public interface IDirectoryDataAccess
    {
        void SaveToFile(string path, string content);
        string LoadFromFile(string path);
    }
}
