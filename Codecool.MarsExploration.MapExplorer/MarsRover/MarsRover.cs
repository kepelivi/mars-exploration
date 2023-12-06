using System.Xml.Schema;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover
{
    public class MarsRover {
        public string Id { get; }
        public Coordinate Position { get; set; }
        public int Sight { get; }
        public Dictionary<Coordinate, (string resource, Coordinate roverPosition)> ResourcesCollection { get; set; }

        public List<Coordinate> PastMovements { get; set; }

        public MarsRover(string id, Coordinate position, int sight)
        {
            Id = id;
            Position = position;
            Sight = sight;
            ResourcesCollection = new Dictionary<Coordinate, (string, Coordinate)> ();
            PastMovements = new List<Coordinate>();
        }
    }
}
