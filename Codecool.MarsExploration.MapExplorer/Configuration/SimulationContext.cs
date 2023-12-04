using Codecool.MarsExploration.MapExplorer.Exploration;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Configuration;

public class SimulationContext
{
    private int _numberOfSteps;
    private int _maxNumOfSteps;
    private MarsRover.MarsRover _rover;
    private Coordinate _landingCoordinate;
    private Map _map;
    private IEnumerable<string> _resourcesToMonitor;
    private ExplorationOutcome _outcome;

    public SimulationContext(int maxNumOfSteps, MarsRover.MarsRover rover, Coordinate landingCoordinate, Map map,
        IEnumerable<string> resourcesToMonitor)
    {
        _maxNumOfSteps = maxNumOfSteps;
        _rover = rover;
        _landingCoordinate = landingCoordinate;
        _map = map;
        _resourcesToMonitor = resourcesToMonitor;
    }

    public void SetOutcome(ExplorationOutcome outcome)
    {
        _outcome = outcome;
    }
}