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

        Assert.That(_configurationValidator.IsLandingSpotValid(testMap, testLandingCoordinate), Is.False);

    }
    
    [Test]
    public void ValidatorInvalidMapFileTest()
    {
        Assert.That(_configurationValidator.IsMapFileValid($"{WorkDir}\\Resources\\test-map.map"), Is.False);
    }
    
    [Test]
    public void ValidatorInvalidResourceTest()
    {
        var testResources = new List<string>();
        
        Assert.That(_configurationValidator.IsThereResources(testResources), Is.False);
    }
    
    [Test]
    public void ValidatorInvalidTimeOutTest()
    {
        const int testTimeOut = 0;
        
        Assert.That(_configurationValidator.IsTimeOutGreaterThenZero(testTimeOut), Is.False);
    }

    [Test]
    public void ValidConfigurationTest()
    {
        var testMap = new Map(new [,] { { " ", " ", " "},{ "&", " ", " "},{ " ", " ", "#"} }, true) ;
        var testLandingCoordinate = new Coordinate(1,1);
        var testResources = new List<string>{"&", "#"};
        const int testTimeOut = 10;
        var testConfig = new Configuration($"{WorkDir}\\TestResources\\test-map.map", testLandingCoordinate, testResources, testTimeOut);
        
        Assert.That(_configurationValidator.Validate(testMap, testConfig), Is.True);
    }

}