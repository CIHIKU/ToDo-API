using MongoDB.Driver;
using ToDo_API.Models;
using ToDo_API.Repositories;

namespace ToDo_API.Services;

public static class ServiceExtensions
{
    public static void AddMongoDBService(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnectionURI = configuration.GetSection("ToDoDatabase:ConnectionURI").Value!;
        var mongoDatabaseName = configuration.GetSection("ToDoDatabase:DatabaseName").Value!;
        services.AddSingleton<IMongoDBService>(new MongoDBService(mongoConnectionURI, mongoDatabaseName));
    }

    public static void AddToDoServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IToDoRepository>(serviceProvider =>
        {
            var mongoService = serviceProvider.GetRequiredService<IMongoDBService>();
            var toDoCollectionName = configuration.GetSection("ToDoDatabase:ToDoCollection").Value!;
            var toDoCollection = mongoService.GetCollection<ToDo>(toDoCollectionName);
            return new ToDoRepository(toDoCollection);
        });
        
        services.AddScoped<IToDoService, ToDoService>();
    }

    public static void AddUserServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository>(serviceProvider =>
        {
            var mongoService = serviceProvider.GetRequiredService<IMongoDBService>();
            var userCollectionName = configuration.GetSection("ToDoDatabase:UserCollection").Value!;
            var userCollection = mongoService.GetCollection<User>(userCollectionName);
            return new UserRepository(userCollection);
        });
    }
}