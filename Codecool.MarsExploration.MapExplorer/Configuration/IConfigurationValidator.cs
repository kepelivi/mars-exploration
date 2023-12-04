using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Configuration;

public interface IConfigurationValidator
{
    bool Validate(Configuration config);

    bool IsLandingSpotValid(Map map, Coordinate landingCoordinate);

    bool IsMapFileValid(string filePath);

    bool IsThereResources(IEnumerable<string> resources);

    bool IsTimeOutGreaterThenZero(int timeout);
}