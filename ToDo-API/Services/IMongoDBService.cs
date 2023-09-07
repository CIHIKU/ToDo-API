using MongoDB.Driver;

namespace ToDo_API.Services;

public interface IMongoDBService : IService
{
    IMongoCollection<T> GetCollection<T>(string collectionName);
}