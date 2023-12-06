using Codecool.MarsExploration.MapExplorer.Exploration;
using Microsoft.Data.Sqlite;

namespace Codecool.MarsExploration.MapExplorer.Repository;

public class SimulationRepository : ISimulationRepository
{
    private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;
    private static readonly string FilePath = $"{WorkDir}/Resources/simulation.db";

    public SimulationRepository()
    {
        CreateDbIfNotExist();
        CreateTable();
    }

    private static void CreateDbIfNotExist()
    {
        if (!File.Exists(FilePath))
        {
            File.Create(FilePath);
        }
    }

    private static SqliteConnection GetPhysicalDbConnection()
    {
        var dbConnection = new SqliteConnection($"Data Source ={FilePath};Mode=ReadWrite");
        dbConnection.Open();
        return dbConnection;
    }

    private static void ExecuteNonQuery(string query)
    {
        using var connection = GetPhysicalDbConnection();
        using var command = GetCommand(query, connection);
        command.ExecuteNonQuery();
    }

    private static SqliteCommand GetCommand(string query, SqliteConnection connection)
    {
        return new SqliteCommand
        {
            CommandText = query,
            Connection = connection
        };
    }

    private static void CreateTable()
    {
        const string createQuery = @$"CREATE TABLE if NOT EXISTS simulations (
                                        id INTEGER PRIMARY KEY,
                                        date_of_the_simulation TIMESTAMP NOT NULL, 
                                        number_of_steps INTEGER NOT NULL,
                                        amount_of_resources INTEGER,
                                        outcome_of_the_simulation VARCHAR NOT NULL
                                   )";
        ExecuteNonQuery(createQuery);
    }

    public void Add(DateTime timestamp, int steps, int resources, ExplorationOutcome? outcome)
    {
        var query = @$"INSERT INTO simulations (date_of_the_simulation,number_of_steps,amount_of_resources,outcome_of_the_simulation)
                        VALUES ('{timestamp} ', '{steps} ', '{resources}', '{outcome}')";
        ExecuteNonQuery(query);
    }

    public IEnumerable<Simulation> GetAll()
    {
        var simulations = new List<Simulation>();
        const string query = @$"SELECT * FROM simulations ";
        using var connection = GetPhysicalDbConnection();
        using var command = GetCommand(query, connection);
        
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var simulation = new Simulation(reader.GetInt32(0), reader.GetDateTime(1), reader.GetInt32(2),
                reader.GetInt32(3), reader.GetString(4));
            simulations.Add(simulation);
        }

        return simulations;
    }

    public Simulation GetById(int id)
    { 
        var query = @$"SELECT * FROM simulations WHERE id = '{id}'";
        using var connection = GetPhysicalDbConnection();
        using var command = GetCommand(query, connection);
        
        using var reader = command.ExecuteReader();

        return new Simulation(reader.GetInt32(0), reader.GetDateTime(1), reader.GetInt32(2), reader.GetInt32(3),
            reader.GetString(4));
    }

    public void Delete(int id)
    {
        try
        {
            var query = @$"DELETE FROM simulations WHERE id = '{id}'";
            ExecuteNonQuery(query);
            
            Console.WriteLine("the simulation successfully deleted");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public void DeleteAll()
    {
        try
        {
            const string query = @$"DELETE FROM simulations ";
            ExecuteNonQuery(query);
            Console.WriteLine("Simulations deleted");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
    }
}