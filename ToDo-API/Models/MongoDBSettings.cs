namespace ToDo_API.Models;

public class MongoDBSettings
{
    public string ConnectionURI { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string UserCollection { get; set; } = null!;
    public string ToDoCollection { get; set; } = null!;
}