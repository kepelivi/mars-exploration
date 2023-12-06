using Codecool.MarsExploration.MapExplorer.Configuration;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public class SuccessAnalyzer : IOutcomeAnalyzer
{
    public bool Analyze(SimulationContext context)
    {
        return MineralWithin5CoordinatesOfWater(context) || Total4MineralsAnd3Waters(context);
    }

    private static bool MineralWithin5CoordinatesOfWater(SimulationContext context)
    {
        var rover = context.Rover;

        return rover.ResourcesCollection
                .Where(resource => resource.Value.resource == "*")
                .Any(resource => rover.ResourcesCollection
                    .Where(resource2 => resource2.Value.resource == "%")
                    .Any(resource2 => CalculateCoordinateDistance(resource.Key, resource2.Key) <= 5));
    }

    private static bool Total4MineralsAnd3Waters(SimulationContext context)
    {
        var mineralCount = 0;
        var waterCount = 0;
        var rover = context.Rover;

        foreach (var resource in rover.ResourcesCollection)
        {
            switch (resource.Value.resource)
            {
                case "*":
                    waterCount++;
                    break;
                case "%":
                    mineralCount++;
                    break;
            }
        }

        return (mineralCount >= 4 && waterCount >= 3);
    }

    private static int CalculateCoordinateDistance(Coordinate coord1, Coordinate coord2)
    {
        var xDiff = Math.Abs(coord1.X - coord2.X);
        var yDiff = Math.Abs(coord1.Y - coord2.Y);
        return (xDiff + yDiff);
    }
}