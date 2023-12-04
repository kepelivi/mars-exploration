using Codecool.MarsExploration.MapExplorer.MapLoader;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorerTest;

public class MapLoaderTest
{
    private static IMapLoader _mapLoader;
    private static string _testMapString;
    
    [SetUp]
    public void Setup()
    {
        _mapLoader = new MapLoader();
        string WorkDir = AppDomain.CurrentDomain.BaseDirectory;
        _testMapString = $"{WorkDir}" + "/Resources/exploration-0.map";
    }

    [Test]
    public void LoadGeneratesValidMap()
    {
        Assert.That(_mapLoader.Load(_testMapString), Is.TypeOf(typeof(Map)));
    }

    [Test]
    public void LoadGeneratesSameMap()
    {
        Map newMap = _mapLoader.Load(_testMapString);
        string newMapString = newMap.ToString();

        using var sr = new StreamReader(_testMapString);
        string originalMapString = sr.ReadToEnd();
        
        Assert.That(newMapString, Is.EquivalentTo(originalMapString));
    }
}