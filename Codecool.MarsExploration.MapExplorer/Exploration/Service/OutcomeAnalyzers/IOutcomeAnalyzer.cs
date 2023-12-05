using Codecool.MarsExploration.MapExplorer.Configuration;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public interface IOutcomeAnalyzer
{
    public bool Analyze(SimulationContext context);
}