using Codecool.MarsExploration.MapExplorer.Configuration;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapExplorer.MapLoader;
using Codecool.MarsExploration.MapExplorer.MarsRover;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public class ExplorationSimulation
{
    private SimulationContext _context;
    private Coordinate _landingCoordinate;
    private Map _map;
    private MarsRover.MarsRover _rover;

    public ExplorationSimulation(Configuration.Configuration config, ILogger logger, string roverId, int roverSight)
    {
        IConfigurationValidator configurationValidator = new ConfigurationValidator();
        IMapLoader mapLoader = new MapLoader.MapLoader();
        _map = mapLoader.Load(config.MapFile);

        if (configurationValidator.Validate(_map, config))
        {
            _landingCoordinate = config.LandingCoordinate;
        }
        else
        {
            logger.Log("ERROR: invalid configuration!");
            throw new Exception("Invalid configuration!");
        }
        
        var roverPlacer = new RoverPlacer();
        _rover = roverPlacer.PlaceRover(roverId, _map, _landingCoordinate, roverSight);
        _context = new SimulationContext(config.MaxSteps, _rover, _landingCoordinate, _map, config.Resources);
    }
    
    public void RunSimulation()
    {
        
    }

    private void Move()
    {
        
    }

    private void Scan()
    {
        for (int i = -_rover.Sight; i <= _rover.Sight; i++)
        {
            for (int j = -_rover.Sight; j <= _rover.Sight; j++)
            {
                Coordinate coordToCheck = new Coordinate(_rover.Position.X + i, _rover.Position.Y + j);
                if (_context.ResourcesToMonitor.Contains(coordToCheck.ToString()))
                {
                    _rover.ResourcesCollection.Add(coordToCheck, coordToCheck.ToString());
                }
            }
        }
    }

    private void Analyse()
    {
        
    }

    private void Log()
    {
        
    }
}