using MongoDB.Driver;
using ToDo_API.Models;

namespace ToDo_API.Services;

public class MongoDBService : IMongoDBService
{
    private readonly IMongoDatabase _database;
    
    public MongoDBService(string connectionURI, string databaseName)
    {
        var client = new MongoClient(connectionURI);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
    
}