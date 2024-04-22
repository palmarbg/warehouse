using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.MockClasses.DataAccesses
{
    [TestClass]
    public class MockDirectoryLoadDataAccess : IDirectoryDataAccess
    {
        private Dictionary<string, string> _contents;
        public MockDirectoryLoadDataAccess(Dictionary<string, string> contents)
        {
            _contents = contents;
        }
        public string LoadFromFile(string path)
        {
            Debug.WriteLine(path);
            Assert.IsTrue(_contents.ContainsKey(path));
            return _contents[path];
        }

        public void SaveToFile(string path, string content)
        {
            throw new NotImplementedException();
        }
    }
}
