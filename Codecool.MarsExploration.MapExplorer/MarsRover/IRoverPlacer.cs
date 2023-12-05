using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapExplorer.MarsRover
{
    public interface IRoverPlacer
    {
        MarsRover PlaceRover(string id, Map map, Coordinate shipPosition, int sight);
    }
}
