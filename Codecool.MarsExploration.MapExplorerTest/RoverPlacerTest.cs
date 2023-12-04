using Codecool.MarsExploration.MapExplorer.MapLoader;
using Codecool.MarsExploration.MapExplorer.MarsRover;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorerTest;


public class RoverPlacerTest
    {
        private readonly IMapLoader _mapLoader = new MapLoader();
        private readonly RoverPlacer _roverPlacer = new RoverPlacer();
        private static readonly string _workDir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _filePath = $"{_workDir}" + "/Resources/exploration-0.map";

        [Test]
        public void PlaceRoverValidInput()
        {
            var map = _mapLoader.Load(_filePath);
            var testrover = _roverPlacer.PlaceRover("rover-1", map, new Coordinate(2,2), 2);

            Assert.That(map.GetByCoordinate(testrover.Position), Is.EqualTo(" "));
        }

        [Test]
        public void PlaceRoverInvalidInput()
        {
            var map = _mapLoader.Load(_filePath);
            try
            {
                var testrover = _roverPlacer.PlaceRover("rover-1", map, new Coordinate(16, 26), 2);

            }
            catch(Exception ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Couldn't find free place around the ship."));

            }
        }

}

