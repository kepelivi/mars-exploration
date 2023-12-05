using Codecool.MarsExploration.MapExplorer.Configuration;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Configuration.Service;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorerTest;

public class ConfigurationValidatorTest
{
    private readonly IConfigurationValidator _configurationValidator = new ConfigurationValidator();
    private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;

    [Test]
    public void ValidatorLandingSpotNotEmptyTest()
    {
        var testMap = new Map(new string[,] { { "&", "#", "#"},{ "&", "#", "#"},{ "&", "#", "#"} }, true) ;
        var testLandingCoordinate = new Coordinate(2,2);
        var testResources = new List<string>{"&", "#"};
        const int testTimeOut = 10;
        var testConfig = new Configuration($"{WorkDir}" + "/TestResources/test-map.map", testLandingCoordinate, testResources, testTimeOut);

        Assert.That(_configurationValidator.Validate(testMap, testConfig), Is.False);

    }
    
    [Test]
    public void ValidatorInvalidMapFileTest()
    {
        var testMap = new Map(new [,] { { " ", " ", " "},{ "&", " ", " "},{ " ", " ", "#"} }, true) ;
        var testLandingCoordinate = new Coordinate(1,1);
        var testResources = new List<string>{"&", "#"};
        const int testTimeOut = 10;
        var testConfig = new Configuration($"{WorkDir}" + "/TestResources/nonexistent-map.map", testLandingCoordinate, testResources, testTimeOut);
        
        Assert.That(_configurationValidator.Validate(testMap, testConfig),Is.False);
    }
    
    [Test]
    public void ValidatorInvalidResourceTest()
    {
        var testMap = new Map(new [,] { { " ", " ", " "},{ "&", " ", " "},{ " ", " ", "#"} }, true) ;
        var testLandingCoordinate = new Coordinate(1,1);
        const int testTimeOut = 10;
        var testResources = new List<string>();
        var testConfig = new Configuration($"{WorkDir}" + "/TestResources/test-map.map", testLandingCoordinate, testResources, testTimeOut);
        
        Assert.That(_configurationValidator.Validate(testMap, testConfig), Is.False);
    }
    
    [Test]
    public void ValidatorInvalidTimeOutTest()
    {
        var testMap = new Map(new [,] { { " ", " ", " "},{ "&", " ", " "},{ " ", " ", "#"} }, true) ;
        var testLandingCoordinate = new Coordinate(1,1);
        var testResources = new List<string>{"&", "#"};
        const int testTimeOut = 0;
        var testConfig = new Configuration($"{WorkDir}" + "/TestResources/test-map.map", testLandingCoordinate, testResources, testTimeOut);
        
        Assert.That(_configurationValidator.Validate(testMap, testConfig), Is.False);
    }

    [Test]
    public void ValidConfigurationTest()
    {
        var testMap = new Map(new [,] { { " ", " ", " "},{ "&", " ", " "},{ " ", " ", "#"} }, true) ;
        var testLandingCoordinate = new Coordinate(1,1);
        var testResources = new List<string>{"&", "#"};
        const int testTimeOut = 10;
        var testConfig = new Configuration($"{WorkDir}" + "/TestResources/test-map.map", testLandingCoordinate, testResources, testTimeOut);
        
        Assert.That(_configurationValidator.Validate(testMap, testConfig), Is.True);
    }

}