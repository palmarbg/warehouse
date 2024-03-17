using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RobotokModel.Persistence
{
    public static class Persistence
    {
        // Konfiguráció elmentésére szerintem nem lesz szükség, még egyeztetni kell róla
        /*public static void SaveConfig(Config config, string path)
        {

        }*/
        public static Config LoadConfig(string path)
        {
            if(!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            else
            {
                string jsonString = File.ReadAllText(path);
                ConfigS? configS = JsonSerializer.Deserialize<ConfigS>(jsonString);
                if(configS == null)
                {
                    throw new JSonError("Serialization of config file was unsuccesful!");
                }
                else
                {
                    
                    // TODO: meg kell nézni, hogy pontosan mi lesz a fájl elérése, jelenleg az exe mellé kell tenni
                    // TODO: hibakezelés hibás path esetén
                    configS.mapFile = Directory.GetCurrentDirectory() + "/" + configS.mapFile;
                    configS.agentFile = Directory.GetCurrentDirectory() + "/" + configS.agentFile;
                    configS.taskFile = Directory.GetCurrentDirectory() + "/" + configS.taskFile;

                    // creating Config type, that contains useful information
                    Config config = new Config();

                    // parsing RevealedTaskCount
                    config.RevealedTaskCount = configS.numTasksReveal;

                    // parsing DistributionStrategy
                    switch (configS.taskAssignmentStrategy)
                    {
                        case "roundrobin":
                            config.DistributionStrategy = Strategy.RoundRobin;
                            break;
                        case "a*":
                            config.DistributionStrategy = Strategy.AStar; 
                            break;
                        default:
                            config.DistributionStrategy = Strategy.RoundRobin;
                            break;
                    }

                    // parsing RobotCount
                    config.RobotCount = configS.teamSize;

                    // parsing MapHeight, MapWidth, Map
                    string[] map = File.ReadAllText(configS.mapFile).Split('\n');
                    // map[0]: type octile nem tudjuk mit jelent, nem használjuk
                    config.MapHeight = int.Parse(map[1].Split(' ')[1]);
                    config.MapWidth  = int.Parse(map[2].Split(' ')[1]);
                   for(int i = 3; i < map.Length; i++)
                   {
                        string r = map[i];
                        // TODO: kicserélni a nem '.' karatereket '@'-ra
                        List<Char> temp = r.ToList();
                        temp.Remove('\n');
                        config.Map.Add(temp);
                   }

                    // parsing robot positions
                    string[] robots = File.ReadAllText(configS.agentFile).Split('\n');
                    int robotCount = int.Parse(robots[0]);
                    for(int i = 1; i <= robotCount; i++)
                    {
                        int intPos = int.Parse(robots[i]);
                        Position p = new Position();
                        p.X = intPos / config.MapWidth;
                        p.Y = intPos % config.MapWidth;
                        if(p.X > 0) { p.X--; }
                        if(p.Y > 0) { p.Y--; }
                        config.RobotPositions.Add(p);
                    }

                    // parsing GoalPositions
                    string[] goals = File.ReadAllText(configS.taskFile).Split('\n');
                    int goalCount = int.Parse(goals[0]);
                    for (int i = 1; i <= goalCount; i++)
                    {
                        int intPos = int.Parse(goals[i]);
                        Position p = new Position();
                        p.X = intPos / config.MapWidth;
                        p.Y = intPos % config.MapWidth;
                        if (p.X > 0) { p.X--; }
                        if (p.Y > 0) { p.Y--; }
                        config.GoalPositions.Add(p);
                    }

                    return config;
                }

                
            }
        }

        public static void SaveLog(Log log, string path) 
        {
            // prot 2
        }
        public static Log LoadLog(string path)
        {
            return new Log();
        }
    }
}
