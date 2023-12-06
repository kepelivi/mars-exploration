using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Service;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover
{
    public class RoverPlacer : IRoverPlacer
    {

        public MarsRover PlaceRover(string id, Map map, Coordinate shipPosition, int sight)
        {
            var roverCoordinate = AvailableCoordinate(map, shipPosition);
            var rover = new MarsRover(id, roverCoordinate, sight);
            return rover;
        }

        private static Coordinate AvailableCoordinate(Map map, Coordinate shipCoords)
        {
            var coordinateCalculator = new CoordinateCalculator();

            var coords = coordinateCalculator.GetAdjacentCoordinates(shipCoords, map.Dimension);

            foreach (var coord in coords)
            {
                if (map.IsEmpty(coord)) return coord;
            }
            throw new Exception("Couldn't find free place around the ship.");
        }
    }
}
