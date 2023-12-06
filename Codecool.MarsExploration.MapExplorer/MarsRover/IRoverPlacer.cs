using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;


namespace Codecool.MarsExploration.MapExplorer.MarsRover
{
    public interface IRoverPlacer
    {
        MarsRover PlaceRover(string id, Map map, Coordinate shipPosition, int sight);
    }
}
