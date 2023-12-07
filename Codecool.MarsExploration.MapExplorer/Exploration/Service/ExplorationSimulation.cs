using Codecool.MarsExploration.MapExplorer.Configuration;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapExplorer.MapLoader;
using Codecool.MarsExploration.MapExplorer.MarsRover;
using Codecool.MarsExploration.MapExplorer.Repository;
using Codecool.MarsExploration.MapExplorer.UI;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Service;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public class ExplorationSimulation
{
    private readonly Random _random = new ();
    private readonly SimulationContext _context;
    private readonly ILogger _logger;
    private readonly ISimulationRepository _repository = new SimulationRepository();

    public ExplorationSimulation(Configuration.Configuration config, ILogger logger, string roverId, int roverSight)
    {
        IConfigurationValidator configurationValidator = new ConfigurationValidator();
        IMapLoader mapLoader = new MapLoader.MapLoader();
        _logger = logger;

        if (configurationValidator.Validate(mapLoader.Load(config.MapFile), config))
        {
           var roverPlacer = new RoverPlacer();
            _context = new SimulationContext(
                config.MaxSteps, 
                roverPlacer.PlaceRover(roverId, mapLoader.Load(config.MapFile), config.LandingCoordinate, roverSight), 
                config.LandingCoordinate,
                mapLoader.Load(config.MapFile), 
                config.Resources
                ); 
        }
        else
        {
            _logger.Log("ERROR: invalid configuration!");
            throw new Exception("Invalid configuration!");
        }
    }
    
    public void RunSimulation(List<Action> simulationSteps)
    {
        for (var i = 1; i <= _context.MaxNumOfSteps; i++)
        {
            foreach (var action in simulationSteps)
            {
                action();
            }

            if (_context.Outcome == null) continue;
            _repository.Add(DateTime.Now, i,_context.Rover.ResourcesCollection.Count, _context.Outcome);
            Console.WriteLine($"Simulation ended with an outcome of {_context.Outcome.ToString()}");
            SmartMoveBack();
            return;
        }
    }

    public void Move()
    {
        var coordinateCalculator = new CoordinateCalculator();
        var emptyCoordinates = coordinateCalculator
            .GetAdjacentCoordinates(_context.Rover.Position, _context.Map.Dimension)
            .Where(coordinate => _context.Map.IsEmpty(coordinate))
            .ToList();

        var newPosition = emptyCoordinates[_random.Next(0, emptyCoordinates.Count)];

        _context.Rover.PastMovements.Add(_context.Rover.Position);
        _context.Rover.Position = new Coordinate(newPosition.X, newPosition.Y);
        _context.NumberOfSteps++;
    }

    public void SmartMove()
    {
        var coordinateCalculator = new CoordinateCalculator();
        var emptyCoordinates = coordinateCalculator.GetAdjacentCoordinates(_context.Rover.Position, _context.Map.Dimension)
            .Where(c => _context.Map.IsEmpty(c)).ToList();

        var availableCoordinates = emptyCoordinates.Where(c => !_context.Rover.PastMovements.Contains(c)).ToList();

        _context.Rover.PastMovements.Add(_context.Rover.Position);

        var resourceFoundInLastStep = _context.Rover.Position;
        var coord = _context.Rover.Position;

        if (_context.Rover.ResourcesCollection.Count != 0)
        {
            resourceFoundInLastStep = _context.Rover.ResourcesCollection
                .FirstOrDefault(resource => resource.Value.roverPosition.X == _context.Rover.PastMovements[^1].X &&
                resource.Value.roverPosition.Y == _context.Rover.PastMovements[^1].Y).Key;

            if (resourceFoundInLastStep != null)
            {
                coord = availableCoordinates.Aggregate(_context.Rover.Position, (shortest, next) =>
                    Math.Abs(next.X - resourceFoundInLastStep.X) + Math.Abs(next.Y - resourceFoundInLastStep.Y) <
                    Math.Abs(shortest.X - resourceFoundInLastStep.X) + Math.Abs(shortest.Y - resourceFoundInLastStep.Y) ? next : shortest
                );
            }
        }
        
        if (availableCoordinates.Count == 0)
        {
            _context.Rover.Position = emptyCoordinates[_random.Next(0, emptyCoordinates.Count)];
        }
        else if (_context.Rover.Position == coord)
        {
            _context.Rover.Position = availableCoordinates[_random.Next(0, availableCoordinates.Count)];
        }
        else
        {
            _context.Rover.Position = coord;
        }
        _context.NumberOfSteps++;
    }

    private void MoveBack()
    {
        _context.Rover.Position = _context.LandingCoordinate;
    }

    private void SmartMoveBack()
    {
        var symbolsToConsoleColors = new Dictionary<string, ConsoleColor>()
        {
            {"#", ConsoleColor.Black},
            {"&", ConsoleColor.DarkYellow},
            {"%", ConsoleColor.Red},
            {"*", ConsoleColor.Blue}
        };
        var ui = new MarsExplorationUI(symbolsToConsoleColors);
        ui.DisplayMap(_context.Map, _context.Rover.Position, _context.LandingCoordinate);
        var shortestPath = new ShortestPathFinder(_context.Map).FindShortestPath(_context.Rover.Position, _context.LandingCoordinate);
        ui.DisplayMap(_context.Map, _context.Rover.Position, _context.LandingCoordinate, shortestPath);
    }

    public void Scan()
    {
        for (var i = -_context.Rover.Sight; i <= _context.Rover.Sight; i++)
        {
            for (var j = -_context.Rover.Sight; j <= _context.Rover.Sight; j++)
            {
                var coordToCheck = new Coordinate(Math.Min(Math.Max(_context.Rover.Position.X + i, 0), _context.Map.Dimension-1), Math.Min(Math.Max(_context.Rover.Position.Y + j, 0), _context.Map.Dimension-1));
                if (!_context.ResourcesToMonitor.Contains(_context.Map.GetByCoordinate(coordToCheck))) continue;
                if (_context.Rover.ResourcesCollection.ContainsKey(coordToCheck)) continue;
                Console.WriteLine($"{_context.Map.GetByCoordinate(coordToCheck)} resources found at {coordToCheck}");
                _context.Rover.ResourcesCollection.Add(coordToCheck, (_context.Map.GetByCoordinate(coordToCheck), _context.Rover.Position)!);
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
        _logger.Log(_context.Outcome == null
            ? $"Rover {_context.Rover.Id} is at coordinates {_context.Rover.Position.X},{_context.Rover.Position.Y}. It has completed {_context.NumberOfSteps} out of {_context.MaxNumOfSteps}. The collection includes {_context.Rover.ResourcesCollection.Count} resources"
            : $"Outcome {_context.Outcome.ToString()} reached!");
    }
}