using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Configuration.Service;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapExplorer.MarsRover
{
    public class RoverPlacer
    {
        public MarsRover PlaceRover(string Id, Map map, Coordinate shipPosition, int sight)
        {
            var roverCoordinate = AvailableCoordinate(map, shipPosition);
            MarsRover rover = new MarsRover(Id, roverCoordinate, sight, new Dictionary<Coordinate, MapElement>());
            return rover;
        }

        private Coordinate AvailableCoordinate(Map map, Coordinate shipCoords)
        {
            Coordinate[] coords = {
                new Coordinate(shipCoords.X + 1, shipCoords.Y + 1),
                new Coordinate(shipCoords.X + 1, shipCoords.Y),
                new Coordinate(shipCoords.X + 1, shipCoords.Y -1),
                new Coordinate(shipCoords.X, shipCoords.Y + 1),
                new Coordinate(shipCoords.X, shipCoords.Y -1),
                new Coordinate(shipCoords.X - 1, shipCoords.Y),
                new Coordinate(shipCoords.X - 1, shipCoords.Y + 1),
                new Coordinate(shipCoords.X -1, shipCoords.Y - 1)
            };
            foreach (var coord in coords)
            {
                if (map.IsEmpty(coord)) return coord;
            }
            throw new Exception("Couldn't find free place around the ship.");
        }
    }
}
