using Model.Executors;
using Model.Interfaces;
using Persistence.DataAccesses;
using Persistence.DataTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Test.MockClasses.Loggers;

namespace Test.Utils
{
    public abstract class TestByLogClassBase
    {
        protected ConfigDataAccess _configDataAccess = null!;
        protected Uri _uriBase = null!;

        protected SimulationData _simulationData = null!;
        protected List<Robot> _robots = null!;

        protected void TestInitBase(string directoryName, string configfileName)
        {
            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("Test"));
            Uri baseUri = new Uri(path);

            _uriBase = new Uri(baseUri, $"Test/TestFiles/{directoryName}/");

            Uri fileUri = new Uri(_uriBase, configfileName);
            string filePath = fileUri.AbsolutePath;

            Assert.IsTrue(File.Exists(filePath));

            _configDataAccess = new(filePath, new DirectoryDataAccess());

            _simulationData = _configDataAccess.GetInitialSimulationData();

            _robots = _simulationData.Robots;
        }

        protected bool AreFilesEqual(string file1Path, string file2Path)
        {
            byte[] file1Hash, file2Hash;

            using (SHA1 sha = SHA1.Create())
            {
                file1Hash = sha.ComputeHash(File.ReadAllBytes(file1Path));
                file2Hash = sha.ComputeHash(File.ReadAllBytes(file2Path));
            }

            return StructuralComparisons.StructuralEqualityComparer.Equals(file1Hash, file2Hash);
        }

        protected void TestResult(string resultPath, string testPath)
        {
            string testfilePath = new Uri(_uriBase, testPath).AbsolutePath;
            Assert.IsTrue(AreFilesEqual(resultPath, testfilePath));
        }

    }
}
