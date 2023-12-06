using Codecool.MarsExploration.MapExplorer.Exploration.Service;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer;

class Program
{
    private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;

    public static void Main(string[] args)
    {
        var mapFile = $@"{WorkDir}/Resources/exploration-0.map";
        var landingSpot = new Coordinate(0, 0);
        var resources = new List<string> { "*", "%" };
        const int timeOut = 1000;
        const string roverId = "Rover1";
        const int roverSight = 3;

        var config = new Configuration.Configuration(mapFile, landingSpot, resources, timeOut);
        ILogger logger = new ConsoleLogger();
        var simulation = new ExplorationSimulation(config, logger, roverId, roverSight);
        var steps = new List<Action>()
        {
            simulation.SmartMove,
            simulation.Scan,
            simulation.Analyse,
            simulation.Log
        };
        
        simulation.RunSimulation(steps);
    }
}
