using Codecool.MarsExploration.MapExplorer.Configuration;
using Codecool.MarsExploration.MapExplorer.Exploration.Service;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer;

class Program
{
    private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;

    public static void Main(string[] args)
    {
        string mapFile = $@"{WorkDir}/Resources/exploration-0.map";
        Coordinate landingSpot = new Coordinate(2, 2);
        var map = new MapLoader.MapLoader().Load(mapFile);
        var resources = new List<string> { "*", "%" };
        int timeOut = 1000;
        string roverId = "Rover1";
        int roverSight = 3;

        var config = new Configuration.Configuration(mapFile, landingSpot, resources, timeOut);
        ILogger logger = new ConsoleLogger();
        var simulation = new ExplorationSimulation(config, logger, roverId, roverSight);
        List<Action> steps = new List<Action>()
        {
            simulation.SmartMove,
            simulation.Scan,
            simulation.Analyse,
            simulation.Log
        };
        
        simulation.RunSimulation(steps);
    }
}
