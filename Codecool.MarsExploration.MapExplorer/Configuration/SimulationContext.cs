using Codecool.MarsExploration.MapExplorer.Exploration;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Configuration;

public class SimulationContext
{
    public int NumberOfSteps;
    public readonly int MaxNumOfSteps;
    public readonly MarsRover.MarsRover Rover;
    public readonly Coordinate LandingCoordinate;
    public readonly Map Map;
    public readonly IEnumerable<string> ResourcesToMonitor;
    public ExplorationOutcome? Outcome;

    public SimulationContext(int maxNumOfSteps, MarsRover.MarsRover rover, Coordinate landingCoordinate, Map map,
        IEnumerable<string> resourcesToMonitor)
    {
        MaxNumOfSteps = maxNumOfSteps;
        Rover = rover;
        LandingCoordinate = landingCoordinate;
        Map = map;
        ResourcesToMonitor = resourcesToMonitor;
        Outcome = null;
    }

    public void SetOutcome(ExplorationOutcome outcome)
    {
        Outcome = outcome;
    }
}