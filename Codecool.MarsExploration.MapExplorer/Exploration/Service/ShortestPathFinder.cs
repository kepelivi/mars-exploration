using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Service;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public class ShortestPathFinder
{
    private readonly Map _map;
    private readonly CoordinateCalculator _coordinateCalculator = new CoordinateCalculator();

    public ShortestPathFinder(Map map)
    {
        _map = map;
    }

    public IEnumerable<Coordinate> FindShortestPath(Coordinate start, Coordinate goal)
    {
        IList<NodeForPathSearch> addedNodes = new List<NodeForPathSearch>();
        addedNodes.Add(new NodeForPathSearch(start, null));
        IList<Coordinate> adjacentUnaddedCoords = GetCoordinatesAdjacentToCoordinates(addedNodes.Select(n => n.Coordinate)).ToList();

        while (adjacentUnaddedCoords.Any(coord => _map.GetByCoordinate(coord) is null or " "))
        {
            foreach (var unaddedCoord in adjacentUnaddedCoords)
            {
                var symbol = _map.GetByCoordinate(unaddedCoord);
                if (symbol is null or " ")
                {
                    var shortestDistanceAdjacentNode = GetShortestDistanceNode(GetAdjacentNodes(unaddedCoord, addedNodes));
                    var nodeToAdd = new NodeForPathSearch(unaddedCoord, shortestDistanceAdjacentNode);
                    addedNodes.Add(nodeToAdd);
                }
            }
            adjacentUnaddedCoords = GetCoordinatesAdjacentToCoordinates(addedNodes.Select(n => n.Coordinate)).ToList();
        }

        foreach (var addedNode in addedNodes)
        {
            if (addedNode.Coordinate == goal)
            {
                return addedNode.GetPathOfCoordsToThisNode();
            }
        }
        
        return new List<Coordinate>();
    }

    private IEnumerable<Coordinate> GetCoordinatesAdjacentToCoordinates(IEnumerable<Coordinate> coordinates)
    {
        IList<Coordinate> adjacentCoords = new List<Coordinate>();

        foreach (var coord in coordinates)
        {
            foreach (var adjacentCoord in _coordinateCalculator.GetAdjacentCoordinates(coord, _map.Dimension, 1))
            {
                if (!coordinates.Contains(adjacentCoord) && !adjacentCoords.Contains(adjacentCoord))
                {
                    adjacentCoords.Add(adjacentCoord);
                }
            }
        }
        
        return adjacentCoords;
    }

    private NodeForPathSearch? GetShortestDistanceNode(IEnumerable<NodeForPathSearch> nodes)
    {
        if (!nodes.Any())
        {
            return null;
        }
        return nodes.MinBy(node => node.GetCurrentShortestDistance());
    }

    private IEnumerable<NodeForPathSearch> GetAdjacentNodes(Coordinate coordinate, IEnumerable<NodeForPathSearch> nodes)
    {
        IList<Coordinate> adjacentCoords =
            new CoordinateCalculator().GetAdjacentCoordinates(coordinate, _map.Dimension, 1).ToList();

        IList<NodeForPathSearch> adjacentNodes = new List<NodeForPathSearch>();

        foreach (var adjacentCoord in adjacentCoords)
        {
            foreach (var node in nodes)
            {
                if (adjacentCoord == node.Coordinate && !adjacentNodes.Contains(node))
                {
                    adjacentNodes.Add(node);
                }
            }
        }
        
        return adjacentNodes;
    }
}