using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration;

public class NodeForPathSearch
{
    public Coordinate Coordinate { get; }
    private readonly IList<NodeForPathSearch> _adjacentNodes;
    private int _currentShortestDistance;
    private NodeForPathSearch? _precedingNode;
    public int Weight { get; }

    public NodeForPathSearch(Coordinate coordinate, NodeForPathSearch? precedingNode, int weight = 1)
    {
        _adjacentNodes = new List<NodeForPathSearch>();
        Coordinate = coordinate;
        Weight = weight;

        if (precedingNode == null)
        {
            _currentShortestDistance = 0;
        }
        else
        {
            _precedingNode = precedingNode;
            _currentShortestDistance = Weight + precedingNode.GetCurrentShortestDistance();
            RefreshDistanceAndPrecedingNode(precedingNode);
        }
    }

    public void RefreshDistanceAndPrecedingNode(NodeForPathSearch newAdjacentNode)
    {
        var distanceWithNewAdjacentNode = Weight + newAdjacentNode.GetCurrentShortestDistance();
        if (_currentShortestDistance > distanceWithNewAdjacentNode)
        {
            _currentShortestDistance = distanceWithNewAdjacentNode;
            _precedingNode = newAdjacentNode;
            foreach (var adjacentNode in _adjacentNodes)
            {
                adjacentNode.RefreshDistanceAndPrecedingNode(this);
            }
        }
        if (!_adjacentNodes.Contains(newAdjacentNode))
        {
            _adjacentNodes.Add(newAdjacentNode);
        }
    }

    public int GetCurrentShortestDistance()
    {
        return _currentShortestDistance;
    }

    public NodeForPathSearch? GetPrecedingNode()
    {
        return _precedingNode;
    }

    public IEnumerable<Coordinate> GetPathOfCoordsToThisNode()
    {
        IList<Coordinate> coordPathToThisNode = new List<Coordinate>() { Coordinate };
        var precedingNode = _precedingNode;
        while (precedingNode != null)
        {
            coordPathToThisNode.Add(precedingNode.Coordinate);
            precedingNode = precedingNode._precedingNode;
        }

        return coordPathToThisNode;
    }

    public override string ToString()
    {
        return $"Coordinates of node: {Coordinate} Weight: {Weight} Current shortest distance from start: {_currentShortestDistance}";
    }

    protected bool Equals(NodeForPathSearch other)
    {
        return Coordinate.Equals(other.Coordinate);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((NodeForPathSearch)obj);
    }

    public override int GetHashCode()
    {
        return Coordinate.GetHashCode();
    }
}