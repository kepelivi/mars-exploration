using Codecool.MarsExploration.MapExplorer.Configuration;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapExplorer.MapLoader;
using Codecool.MarsExploration.MapExplorer.MarsRover;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Service;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public class ExplorationSimulation
{
    private readonly Random _random = new ();
    private SimulationContext _context;
    private Coordinate _landingCoordinate;
    private Map _map;
    private MarsRover.MarsRover _rover;
    private ILogger _logger;

    public ExplorationSimulation(Configuration.Configuration config, ILogger logger, string roverId, int roverSight)
    {
        IConfigurationValidator configurationValidator = new ConfigurationValidator();
        IMapLoader mapLoader = new MapLoader.MapLoader();
        _logger = logger;
        _map = mapLoader.Load(config.MapFile);

        if (configurationValidator.Validate(_map, config))
        {
            _landingCoordinate = config.LandingCoordinate;
        }
        else
        {
            _logger.Log("ERROR: invalid configuration!");
            throw new Exception("Invalid configuration!");
        }
        
        var roverPlacer = new RoverPlacer();
        _rover = roverPlacer.PlaceRover(roverId, _map, _landingCoordinate, roverSight);
        _context = new SimulationContext(config.MaxSteps, _rover, _landingCoordinate, _map, config.Resources);
    }
    
    public void RunSimulation(List<Action> simulationSteps)
    {
        for (int i = 1; i <= _context.MaxNumOfSteps; i++)
        {
            foreach (var action in simulationSteps)
            {
                action();
            }

            if (_context.Outcome != null)
            {
                Console.WriteLine($"Simulation ended with an outcome of {_context.Outcome.ToString()}");
                return;
            }
        }
    }

    public void Move()
    {
        var coordinateCalculator = new CoordinateCalculator();
        var emptyCoordinates = coordinateCalculator
            .GetAdjacentCoordinates(_rover.Position, _map.Dimension)
            .Where(coordinate => _map.IsEmpty(coordinate))
            .ToList();

        var newPosition = emptyCoordinates[_random.Next(0, emptyCoordinates.Count)];

        _rover.PastMovements.Add(_rover.Position);
        _rover.Position = new Coordinate(newPosition.X, newPosition.Y);
        _context.NumberOfSteps++;
    }

    public void MoveSmart()
    {
        //todo: add IsEmpty to Move
        _rover.PastMovements.Add(_rover.Position);

        var coordinateCalculator = new CoordinateCalculator();
        var emptyCoordinates = coordinateCalculator
            .GetAdjacentCoordinates(_rover.Position, _map.Dimension)
            .Where(coordinate => _map.IsEmpty(coordinate))
            .ToList();

        var emptyNotUsedCoordinates = emptyCoordinates
            .Where(coordinate => !_rover.PastMovements.Contains(coordinate))
            .ToList();

        var resourcesFoundInLastStepCoordinates = new List<Coordinate>();

        foreach (KeyValuePair<Coordinate, (string, Coordinate)> item in _rover.ResourcesCollection)
        {
            if(item.Value.Item2 == _rover.PastMovements[_rover.PastMovements.Count - 1]) resourcesFoundInLastStepCoordinates.Add(item.Key);
        }

        var coordinateNearResource = new Coordinate (_map.Dimension + 1, _map.Dimension + 1);

        foreach (var coordinate in emptyNotUsedCoordinates)
        {
            foreach (var item in resourcesFoundInLastStepCoordinates)
            {
                if (Math.Abs(item.X - coordinate.X) + Math.Abs(item.Y - coordinate.Y) < Math.Abs(coordinateNearResource.X - coordinate.X) + Math.Abs(coordinateNearResource.Y - coordinate.Y))
                {
                    coordinateNearResource = coordinate;
                }
            }
        }

        if(coordinateNearResource != new Coordinate(_map.Dimension + 1, _map.Dimension + 1))
        {
            _rover.Position = coordinateNearResource;
        }
        else
        {
            _rover.Position = emptyNotUsedCoordinates[_random.Next(0, emptyNotUsedCoordinates.Count-1)];
        }

        _context.NumberOfSteps++;

    }

    private void MoveBack()
    {
        _rover.Position = _landingCoordinate;
    }

    public void Scan()
    {
        // TODO: Need to check if coordinate even exists
        for (int i = -_rover.Sight; i <= _rover.Sight; i++)
        {
            for (int j = -_rover.Sight; j <= _rover.Sight; j++)
            {
                Coordinate coordToCheck = new Coordinate(Math.Max(_rover.Position.X + i, 0), Math.Max(_rover.Position.Y + j, 0));
                // Console.WriteLine($"[{coordToCheck.X},{coordToCheck.Y}]: {_map.GetByCoordinate(coordToCheck)}");
                if (_context.ResourcesToMonitor.Contains(_map.GetByCoordinate(coordToCheck)))
                {
                    if (!_rover.ResourcesCollection.Keys.Contains(coordToCheck))
                    {
                        _rover.ResourcesCollection.Add(coordToCheck, (_map.GetByCoordinate(coordToCheck), _rover.Position));
                    }
                }
            }
        }
    }

    public void Analyse()
    {
        IOutcomeAnalyzer successAnalyzer = new SuccessAnalyzer();
        IOutcomeAnalyzer timeoutAnalyzer = new TimeoutAnalyzer();

        if (successAnalyzer.Analyze(_context)) _context.Outcome = ExplorationOutcome.Colonizable;
        if (timeoutAnalyzer.Analyze(_context)) _context.Outcome = ExplorationOutcome.Timeout;
    }

    public void Log()
    {
        if (_context.Outcome == null)
        {
            _logger.Log($"Rover {_rover.Id} is at coordinates {_rover.Position.X},{_rover.Position.Y}. It has completed {_context.NumberOfSteps} out of {_context.MaxNumOfSteps}. The collection includes {_rover.ResourcesCollection.Count} resources");
        }
        else
        {
            _logger.Log($"Outcome {_context.Outcome.ToString()} reached!");
        }
    }
}