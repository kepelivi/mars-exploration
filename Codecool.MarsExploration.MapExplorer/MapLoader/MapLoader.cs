using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.MapLoader;

public class MapLoader : IMapLoader
{
    public Map Load(string mapFile)
    {
        var lines = new List<string>();
        using var sr = new StreamReader(mapFile);
        while (sr.Peek() >= 1)
        {
            var line = sr.ReadLine() ?? "";
            lines.Add(line);
        }
        var arr = new string[lines.Count,lines[0].Length];
        for (var i = 0; i < lines.Count; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                arr[i, j] = lines[i][j].ToString();
            }
        }

        return new Map(arr, true);
    }
}