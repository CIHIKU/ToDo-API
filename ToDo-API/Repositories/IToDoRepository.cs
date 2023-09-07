using ToDo_API.Models;

namespace ToDo_API.Repositories;

public interface IToDoRepository : IRepository
{
    public Task<List<ToDo>> GetAllAsync();
    public Task<List<ToDo>> GetByUserIdAsync(string userId);
    public Task<ToDo> GetByIdAsync(string id);
    public Task CreateAsync(ToDo toDo);
    public Task UpdateAsync(string id, ToDo toDo);
    public Task<bool> DeleteAsync(string id);
    public Task ToggleTaskCompletionAsync(string id);
    public Task AddSubTaskAsync(string toDoId, SubTask subTask);
    public Task UpdateSubTaskAsync(string toDoId, string subTaskId, SubTask subTask);
    public Task<bool> RemoveSubTaskAsync(string id, string subTaskId);
    public Task ToggleSubTaskCompletionAsync(string todoId, string subTaskId);

}