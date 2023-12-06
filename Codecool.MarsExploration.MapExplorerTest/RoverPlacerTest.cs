using Codecool.MarsExploration.MapExplorer.MapLoader;
using Codecool.MarsExploration.MapExplorer.MarsRover;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorerTest;


public class RoverPlacerTest
    {
        private readonly IMapLoader _mapLoader = new MapLoader();
        private readonly IRoverPlacer _roverPlacer = new RoverPlacer();
        private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string FilePath = $"{WorkDir}" + "/Resources/exploration-0.map";

        [Test]
        public void PlaceRoverValidInput()
        {
            var map = _mapLoader.Load(FilePath);
            var testRover = _roverPlacer.PlaceRover("rover-1", map, new Coordinate(2,2), 2);

            Assert.That(map.GetByCoordinate(testRover.Position), Is.EqualTo(" "));
        }

        [Test]
        public void PlaceRoverInvalidInput()
        {
            var map = _mapLoader.Load(FilePath);
            try
            {
                var testRover = _roverPlacer.PlaceRover("rover-1", map, new Coordinate(16, 26), 2);
            }
            catch(Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Couldn't find free place around the ship."));

            }
        }

}

