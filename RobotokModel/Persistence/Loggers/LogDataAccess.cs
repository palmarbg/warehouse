using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace RobotokModel.Persistence.Loggers
{
    class LogDataAccess
    {
        private Uri baseUri = null!;
        public Log LoadLog(string path)
        {
            var cucc = path;
            baseUri = new(path);
            string jsonString = File.ReadAllText(path);
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new JsonStringEnumConverter());
            Log? log = JsonSerializer.Deserialize<Log>(jsonString, options) ?? throw new JSonError("Serialization of config file was unsuccesful!");


            return log;
        }
    }
}
