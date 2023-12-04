using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Service;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Configuration;

public class ConfigurationValidator : IConfigurationValidator
{
    public bool Validate(Configuration config)
    {
        return new List<bool>(
            IsLandingSpotValid(), IsMapFileValid(config.MapFile), IsThereResources(config.Resources), IsTimeOutGreaterThenZero(config.MaxSteps))
            .Contains(false) ? true : false;
    }

    public bool IsLandingSpotValid(Map map, Coordinate landingCoordinate)
    {
        return map.Representation[landingCoordinate.X, landingCoordinate.Y] == null && 
               new CoordinateCalculator().GetAdjacentCoordinates(landingCoordinate, map.Dimension).Contains(null);
    }

    public bool IsMapFileValid(string filePath)
    {
        return File.Exists(filePath);
    }

    public bool IsThereResources(IEnumerable<string> resources)
    {
        return resources.Any();
    }

    public bool IsTimeOutGreaterThenZero(int timeout)
    {
        return timeout > 0;
    }
}