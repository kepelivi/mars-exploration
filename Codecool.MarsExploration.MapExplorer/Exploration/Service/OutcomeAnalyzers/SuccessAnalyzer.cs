using System.ComponentModel;
using Codecool.MarsExploration.MapExplorer.Configuration;
using Codecool.MarsExploration.MapExplorer.MarsRover;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public class SuccessAnalyzer : IOutcomeAnalyzer
{
    public bool Analyze(SimulationContext context)
    {
        if (MineralWithin5CoordinatesOfWater(context)) return true;
        if (Total4MineralsAnd3Waters(context)) return true;
        return false;
    }

    private bool MineralWithin5CoordinatesOfWater(SimulationContext context)
    {
        MarsRover.MarsRover rover = context.Rover;

        foreach (var resource in rover.ResourcesCollection)
        {
            if (resource.Value.Item1 == "*")
            {
                foreach (var resource2 in rover.ResourcesCollection)
                {
                    if (resource2.Value.Item1 == "%")
                    {
                        if (CalculateCoordinateDistance(resource.Key, resource2.Key) <= 5) return true;
                    }
                }
            }
        }
        return false;
    }

    private bool Total4MineralsAnd3Waters(SimulationContext context)
    {
        int mineralCount = 0;
        int waterCount = 0;
        MarsRover.MarsRover rover = context.Rover;

        foreach (var resource in rover.ResourcesCollection)
        {
            if (resource.Value.Item1 == "*") waterCount++;
            if (resource.Value.Item1 == "%") mineralCount++;
        }

        return (mineralCount >= 4 && waterCount >= 3);
    }

    private int CalculateCoordinateDistance(Coordinate coord1, Coordinate coord2)
    {
        int xDiff = Math.Abs(coord1.X - coord2.X);
        int yDiff = Math.Abs(coord1.Y - coord2.Y);
        return (xDiff + yDiff);
    }
}