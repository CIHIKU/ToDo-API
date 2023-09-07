using ToDo_API.Models;
using ToDo_API.Repositories;

namespace ToDo_API.Services;

public class ToDoService : IToDoService
{
    private readonly IToDoRepository _toDoRepository;

    public ToDoService(IToDoRepository toDoRepository)
    {
        _toDoRepository = toDoRepository;
    }

    public Task<List<ToDo>> GetAllAsync()
    {
        return _toDoRepository.GetAllAsync();
    }

    public Task<List<ToDo>> GetByUserIdAsync(string userId)
    {
        return _toDoRepository.GetByUserIdAsync(userId);
    }

    public Task<ToDo> GetByIdAsync(string id)
    {
        return _toDoRepository.GetByIdAsync(id);
    }

    public Task CreateAsync(ToDo toDo)
    {
        return _toDoRepository.CreateAsync(toDo);
    }

    public Task UpdateAsync(string id, ToDo toDo)
    {
        return _toDoRepository.UpdateAsync(id, toDo);
    }

    public Task<bool> DeleteAsync(string id)
    {
        return _toDoRepository.DeleteAsync(id);
    }

    public Task ToggleTaskCompletionAsync(string id)
    {
        return _toDoRepository.ToggleTaskCompletionAsync(id);
    }

    public Task AddSubTaskAsync(string toDoId, SubTask subTask)
    {
        return _toDoRepository.AddSubTaskAsync(toDoId, subTask);
    }

    public Task UpdateSubTaskAsync(string toDoId, string subTaskId, SubTask subTask)
    {
        return _toDoRepository.UpdateSubTaskAsync(toDoId, subTaskId, subTask);
    }

    public Task<bool> RemoveSubTaskAsync(string id, string subTaskId)
    {
        return _toDoRepository.RemoveSubTaskAsync(id, subTaskId);
    }

    public Task ToggleSubTaskCompletionAsync(string todoId, string subTaskId)
    {
        return _toDoRepository.ToggleSubTaskCompletionAsync(todoId, subTaskId);
    }
}