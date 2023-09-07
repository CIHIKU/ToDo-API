using MongoDB.Bson;
using MongoDB.Driver;
using ToDo_API.Models;
using ToDo_API.Services;

namespace ToDo_API.Repositories;

public class ToDoRepository : IToDoRepository
{
    private readonly IMongoCollection<ToDo> _toDoCollection;

    public ToDoRepository(IMongoCollection<ToDo> toDoCollection)
    {
        _toDoCollection = toDoCollection;
    }

    public async Task<List<ToDo>> GetAllAsync()
    {
        try
        {
            return await _toDoCollection.Find(new BsonDocument()).ToListAsync();
        }
        catch (Exception e)
        {
            throw new ApplicationException("An error occurred while retrieving all ToDo items.", e);
        }
    }
    
    public async Task<List<ToDo>> GetByUserIdAsync(string userId)
    {
        try
        {
            return await _toDoCollection.Find(todo => todo.UserId == userId).ToListAsync();
        }
        catch (Exception e)
        {
            throw new ApplicationException($"An error occurred while retrieving ToDo items for user ID {userId}.", e);
        }
    }

    public async Task<ToDo> GetByIdAsync(string id)
    {
        try
        {
            return await _toDoCollection.Find(todo => todo.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw new ApplicationException($"An error occurred while retrieving the ToDo item with ID {id}.", e);
        }
    }
    
    public async Task CreateAsync(ToDo toDo)
    {
        try
        {
            if (toDo.SubTasks is { Count: 0 })
            {
                toDo.SubTasks = null;
            }
            await _toDoCollection.InsertOneAsync(toDo);
        }
        catch (Exception e)
        {
            throw new ApplicationException("An error occurred while creating a new ToDo item.", e);
        }
    }

    public async Task UpdateAsync(string id, ToDo toDo)
    {
        try
        {
            var filter = Builders<ToDo>.Filter.Eq("Id", id);
            var updateDefinition = Builders<ToDo>.Update
                .Set(t => t.Title, toDo.Title)
                .Set(t => t.Description, toDo.Description)
                .Set(t => t.IsCompleted, toDo.IsCompleted);
            
            if (toDo.SubTasks.Any())
            {
                updateDefinition = updateDefinition.Set(t => t.SubTasks, toDo.SubTasks);
            }
            
            var updateOptions = new UpdateOptions { IsUpsert = false };
            await _toDoCollection.UpdateOneAsync(filter, updateDefinition, updateOptions);
        }
        catch (Exception e)
        {
            throw new ApplicationException($"An error occurred while updating the ToDo item with ID {id}.", e);
        }
        
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {
            var filter = Builders<ToDo>.Filter.Eq("Id", id);
            var deleteResult = await _toDoCollection.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
        catch (Exception e)
        {
            throw new ApplicationException($"An error occurred while deleting the ToDo item with ID {id}.", e);
        }
        
    }

    public async Task ToggleTaskCompletionAsync(string id)
    {
        try
        {
            var todo = await _toDoCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (todo != null)
            {
                var filter = Builders<ToDo>.Filter.Eq("Id", id);
                var update = Builders<ToDo>.Update.Set("IsCompleted", !todo.IsCompleted);
                await _toDoCollection.UpdateOneAsync(filter, update);
            }
        }
        catch (Exception e)
        {
            throw new ApplicationException($"An error occurred while toggling the completion status of the ToDo item with ID {id}.", e);
        }
        
    }
    
    public async Task AddSubTaskAsync(string toDoId, SubTask subTask)
    {
        try
        {
            var filter = Builders<ToDo>.Filter.Eq("Id", toDoId);
            var update = Builders<ToDo>.Update.Push(t => t.SubTasks, subTask);
            await _toDoCollection.UpdateOneAsync(filter, update);
        }
        catch (Exception e)
        {
            throw new ApplicationException($"An error occurred while adding a subtask to the ToDo item with ID {toDoId}.", e);
        }
    }

    public async Task UpdateSubTaskAsync(string toDoId, string subTaskId, SubTask subTask)
    {
        try
        {
            var filter = Builders<ToDo>.Filter.And(
                Builders<ToDo>.Filter.Eq(t => t.Id, toDoId),
                Builders<ToDo>.Filter.Eq("SubTasks._id", new ObjectId(subTaskId))
            );

            var update = Builders<ToDo>.Update.Set("SubTasks.$", subTask);
            await _toDoCollection.UpdateOneAsync(filter, update);
        }
        catch (Exception e)
        {
            throw new ApplicationException($"An error occurred while updating the subtask with ID {subTaskId} in the ToDo item with ID {toDoId}.", e);
        }
    }
    
    public async Task<bool> RemoveSubTaskAsync(string id, string subTaskId)
    {
        try
        {
            var filter = Builders<ToDo>.Filter.Eq("Id", id);
            var update = Builders<ToDo>.Update.PullFilter(t => t.SubTasks, st => st.Id == subTaskId);
            var updateResult = await _toDoCollection.UpdateOneAsync(filter, update);
            
            var emptyArrayFilter = Builders<ToDo>.Filter.And(
                Builders<ToDo>.Filter.Eq("Id", id),
                Builders<ToDo>.Filter.Size(t => t.SubTasks, 0));
            var unsetUpdate = Builders<ToDo>.Update.Unset(t => t.SubTasks);
            
            await _toDoCollection.UpdateOneAsync(emptyArrayFilter, unsetUpdate);
            
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
        catch (Exception e)
        {
            throw new ApplicationException($"An error occurred while removing the subtask with ID {subTaskId} from the ToDo item with ID {id}.", e);
        }
    }
    
    public async Task ToggleSubTaskCompletionAsync(string todoId, string subTaskId)
    {
        try
        {
            var todo = await _toDoCollection.Find(t => t.Id == todoId).FirstOrDefaultAsync();
            var subTask = todo?.SubTasks.FirstOrDefault(st => st.Id == subTaskId);
            if (subTask != null)
            {
                var filter = Builders<ToDo>.Filter.And(
                    Builders<ToDo>.Filter.Eq(t => t.Id, todoId),
                    Builders<ToDo>.Filter.Eq("SubTasks._id", new ObjectId(subTaskId))
                );
                var update = Builders<ToDo>.Update.Set("SubTasks.$.IsCompleted", !subTask.IsCompleted);
                await _toDoCollection.UpdateOneAsync(filter, update);
            }
        }
        catch (Exception e)
        {
            throw new ApplicationException($"An error occurred while toggling the completion status of the subtask with ID {subTaskId} in the ToDo item with ID {todoId}.", e);
        }
    }
}