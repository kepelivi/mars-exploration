using System.ComponentModel;
using System.Drawing;
using Codecool.MarsExploration.MapExplorer.Configuration;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.UI;

public class MarsExplorationUI
{
    private readonly Dictionary<string, ConsoleColor> _symbolsToConsoleColors;
    private readonly string _roverSymbol;
    private readonly string _landingSiteSymbol;
    private readonly string _pathSymbol;
    // X row, Y column
    
    public MarsExplorationUI(Dictionary<string, ConsoleColor> symbolsToConsoleColors, string roverSymbol = "R", string landingSiteSymbol = "A", string pathSymbol = "=")
    {
        _symbolsToConsoleColors = symbolsToConsoleColors;
        _roverSymbol = roverSymbol;
        _landingSiteSymbol = landingSiteSymbol;
        _pathSymbol = pathSymbol;
    }
    
    public void Menu()
    {
        
    }

    public void DisplayMap(Map map, int numberOfExtraSpaces = 1)
    {
        for (int i = 0; i < map.Representation.GetLength(0); i++)
        {
            for (int j = 0; j < map.Representation.GetLength(1); j++)
            {
                var currentSymbol = map.GetByCoordinate(new Coordinate(i, j));
                DisplayOneSymbol(currentSymbol, numberOfExtraSpaces);
            }
            Console.WriteLine();
        }
    }
    
    public void DisplayMap(Map map, Coordinate rover, Coordinate landingSite, int numberOfExtraSpaces = 1)
    {
        for (int i = 0; i < map.Representation.GetLength(0); i++)
        {
            for (int j = 0; j < map.Representation.GetLength(1); j++)
            {
                var currentCoord = new Coordinate(i, j);
                if (currentCoord == rover)
                {
                    DisplayOneSymbol(_roverSymbol, numberOfExtraSpaces);
                    continue;
                }
                if (currentCoord == landingSite)
                {
                    DisplayOneSymbol(_landingSiteSymbol, numberOfExtraSpaces);
                    continue;
                }
                var currentSymbol = map.GetByCoordinate(currentCoord);
                DisplayOneSymbol(currentSymbol, numberOfExtraSpaces);
            }
            Console.WriteLine();
        }
    }
    
    public void DisplayMap(Map map, Coordinate rover, Coordinate landingSite, IEnumerable<Coordinate> coordPath, int numberOfExtraSpaces = 1)
    {
        if (!coordPath.Any())
        {
            DisplayMap(map, rover, landingSite);
            Console.WriteLine("There is no path from the rover to the landing site");
        }
        
        for (int i = 0; i < map.Representation.GetLength(0); i++)
        {
            for (int j = 0; j < map.Representation.GetLength(1); j++)
            {
                var currentCoord = new Coordinate(i, j);
                if (currentCoord == rover)
                {
                    DisplayOneSymbol(_roverSymbol, numberOfExtraSpaces);
                    continue;
                }
                if (currentCoord == landingSite)
                {
                    DisplayOneSymbol(_landingSiteSymbol, numberOfExtraSpaces);
                    continue;
                }
                if (coordPath.Contains(currentCoord))
                {
                    DisplayOneSymbol(_pathSymbol, numberOfExtraSpaces);
                    continue;
                }
                var currentSymbol = map.GetByCoordinate(currentCoord);
                DisplayOneSymbol(currentSymbol, numberOfExtraSpaces);
            }
            Console.WriteLine();
        }
    }

    private void DisplayOneSymbol(string? symbol)
    {
        if (symbol == null)
        {
            Console.Write(" ");
            return;
        }
        if (symbol == _pathSymbol)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        if (_symbolsToConsoleColors.ContainsKey(symbol))
        {
            Console.ForegroundColor = _symbolsToConsoleColors[symbol];
        }
        Console.Write(symbol);
        Console.ForegroundColor = ConsoleColor.White;
    }

    private void DisplayOneSymbol(string? symbol, int numberOfExtraSpaces)
    {
        DisplayOneSymbol(symbol);
        if (numberOfExtraSpaces > 0)
        {
            for (int i = 0; i < numberOfExtraSpaces; i++)
            {
                Console.Write(" ");
            }
        }
    }
}