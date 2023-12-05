using Codecool.MarsExploration.MapExplorer.Exploration;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Configuration;

public class SimulationContext
{
    public int NumberOfSteps;
    public int MaxNumOfSteps;
    public MarsRover.MarsRover Rover;
    private Coordinate _landingCoordinate;
    private Map _map;
    public readonly IEnumerable<string> ResourcesToMonitor;
    public ExplorationOutcome? Outcome;

    public SimulationContext(int maxNumOfSteps, MarsRover.MarsRover rover, Coordinate landingCoordinate, Map map,
        IEnumerable<string> resourcesToMonitor)
    {
        MaxNumOfSteps = maxNumOfSteps;
        Rover = rover;
        _landingCoordinate = landingCoordinate;
        _map = map;
        ResourcesToMonitor = resourcesToMonitor;
        Outcome = null;
    }

    public void SetOutcome(ExplorationOutcome outcome)
    {
        Outcome = outcome;
    }
}