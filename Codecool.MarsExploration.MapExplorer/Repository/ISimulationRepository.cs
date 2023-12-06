using Codecool.MarsExploration.MapExplorer.Exploration;

namespace Codecool.MarsExploration.MapExplorer.Repository;

public interface ISimulationRepository
{
    void Add(DateTime timestamp, int steps, int resources, ExplorationOutcome? outcome);

    IEnumerable<Simulation> GetAll();

    Simulation GetById(int id);

    void Delete(int id);

    void DeleteAll();

}