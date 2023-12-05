using Codecool.MarsExploration.MapExplorer.Configuration;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public class TimeoutAnalyzer : IOutcomeAnalyzer
{
    public bool Analyze(SimulationContext context)
    {
        return (context.NumberOfSteps >= context.MaxNumOfSteps);
    }
}