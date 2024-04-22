using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DataAccesses
{
    public class DirectoryDataAccess : IDirectoryDataAccess
    {
        public string LoadFromFile(string path)
        {
            return File.ReadAllText(path);
        }

        public void SaveToFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
