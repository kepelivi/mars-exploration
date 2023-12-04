using Codecool.MarsExploration.MapExplorer.Configuration;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer;

class Program
{
    private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;

    public static void Main(string[] args)
    {
        string mapFile = $@"{WorkDir}\Resources\exploration-0.map";
        Coordinate landingSpot = new Coordinate(2, 2);
        var map = new MapLoader.MapLoader().Load(mapFile);
        var resources = new List<string> { "&" };
        var timeOut = 10;

        var config = new Configuration.Configuration(mapFile, landingSpot, resources, timeOut);
        
        Console.WriteLine(new ConfigurationValidator().Validate(map, config));

    }
}
