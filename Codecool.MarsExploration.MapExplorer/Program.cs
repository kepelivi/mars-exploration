using System.Text.RegularExpressions;
using Codecool.MarsExploration.MapExplorer.Exploration;
using Codecool.MarsExploration.MapExplorer.Exploration.Service;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapExplorer.Repository;
using Codecool.MarsExploration.MapExplorer.UI;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Service;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer;

class Program
{
    private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;

    public static void Main(string[] args)
    {
        
        /*
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
        */
        
        //============TESTS====================================================================
        
        var mapFile = $@"{WorkDir}/Resources/exploration-0.map";
        var map = new MapLoader.MapLoader().Load(mapFile);
        
        Console.WriteLine(map.ToString());
        
        var symbolsToConsoleColors = new Dictionary<string, ConsoleColor>()
        {
            {"#", ConsoleColor.Black},
            {"&", ConsoleColor.DarkYellow},
            {"%", ConsoleColor.Red},
            {"*", ConsoleColor.Blue}
        };
        var ui = new MarsExplorationUI(symbolsToConsoleColors);
        
        var nodeStart = new NodeForPathSearch(new Coordinate(4, 4), null);
        var node1 = new NodeForPathSearch(new Coordinate(4, 5), nodeStart);
        var node2 = new NodeForPathSearch(new Coordinate(4, 6), node1);
        var node3 = new NodeForPathSearch(new Coordinate(4, 7), node2);
        var node4 = new NodeForPathSearch(new Coordinate(4, 8), node3);
        var testNodes = new List<NodeForPathSearch>() { nodeStart, node1, node2, node3, node4 };
        
        var testMap = new Map(new [,]
        {
            { "&", "&", null, "&", " ", "#"},
            { "&", " ", " ", " ", " ", "#"},
            { "&", " ", "#", "&", " ", "#"},
            { "&", " ", "#", "&", " ", "#"},
            { "&", " ", " ", " ", " ", "#"},
            { "&", "&", "#", "&", " ", "#"},
        }, true);

        var start = new Coordinate(0, 2);
        var goal = new Coordinate(30, 30);
        
        var path2 = new ShortestPathFinder(map).FindShortestPath(start, goal);
        ui.DisplayMap(map, start, goal, path2);

    }
}
