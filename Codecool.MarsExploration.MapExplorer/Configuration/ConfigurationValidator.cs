using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Service;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Configuration;

public class ConfigurationValidator : IConfigurationValidator
{
    public bool Validate(Map map, Configuration config)
    {
        List<bool> validationResults = new List<bool>()
        {
            IsLandingSpotValid(map, config.LandingCoordinate),
            IsMapFileValid(config.MapFile),
            IsThereResources(config.Resources),
            IsTimeOutGreaterThenZero(config.MaxSteps)
        };

        return validationResults.Contains(false);
    }

    public bool IsLandingSpotValid(Map map, Coordinate landingCoordinate)
    {
        return map.Representation[landingCoordinate.X, landingCoordinate.Y] == null && 
               new CoordinateCalculator().GetAdjacentCoordinates(landingCoordinate, map.Dimension).Contains(null);
    }

    // this file itself might not exist before calling MapLoader.Load() - this creates the file if it does not exist.
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