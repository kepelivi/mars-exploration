using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Configuration;

public record Configuration(string MapFile, Coordinate LandingCoordinate, IEnumerable<string> Resources, int MaxSteps);