using Microsoft.AspNetCore.Mvc;
using ToDo_API.Models;
using ToDo_API.Services;

namespace ToDo_API.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoController: ControllerBase
{
    private readonly IToDoService _toDoService;

    public ToDoController(IToDoService toDoService)
    {
        _toDoService = toDoService;
    }
    [HttpGet]
    public async Task<ActionResult<List<ToDo>>> GetAllAsync()
    {
        return Ok(await _toDoService.GetAllAsync()); 
    }

     [HttpGet("user/{userId}")]
     public async Task<ActionResult<List<ToDo>>> GetByUserIdAsync(string userId)
     {
         return Ok(await _toDoService.GetByUserIdAsync(userId));
     }

     [HttpGet("{id}", Name = "GetById")]
     public async Task<ActionResult<ToDo>> GetByIdAsync(string id)
     {
         var toDo = await _toDoService.GetByIdAsync(id); 
         return Ok(toDo);
     }

     [HttpPost]
     public async Task<IActionResult> CreateAsync([FromBody] ToDo toDo)
     { 
         await _toDoService.CreateAsync(toDo);
         return CreatedAtAction("GetById", new { id = toDo.Id }, toDo);
     }

     [HttpPut("{id}")] 
     public async Task<IActionResult> UpdateAsync(string id, [FromBody] ToDo toDo) 
     { 
         await _toDoService.UpdateAsync(id, toDo); 
         return NoContent();
     }

     [HttpDelete("{id}")]
     public async Task<IActionResult> DeleteAsync(string id)
     {
         var deleted = await _toDoService.DeleteAsync(id);
         if (!deleted)
         { 
             return NotFound();
         }
         return NoContent();
     }

     [HttpPost("{id}/toggle-completion")]
     public async Task<IActionResult> ToggleTaskCompletionAsync(string id)
     {
         await _toDoService.ToggleTaskCompletionAsync(id);
         return NoContent();
     }

     [HttpPost("{toDoId}/subtasks")]
     public async Task<IActionResult> AddSubTaskAsync(string toDoId, [FromBody] SubTask subTask)
     {
            await _toDoService.AddSubTaskAsync(toDoId, subTask);
            return CreatedAtAction("GetById", new { id = toDoId }, subTask);
     }

     [HttpPut("{toDoId}/subtasks/{subTaskId}")]
     public async Task<IActionResult> UpdateSubTaskAsync(string toDoId, string subTaskId, [FromBody] SubTask subTask)
     {
         await _toDoService.UpdateSubTaskAsync(toDoId, subTaskId, subTask);
         return NoContent();
     }

     [HttpDelete("{toDoId}/subtasks/{subTaskId}")]
     public async Task<IActionResult> RemoveSubTaskAsync(string toDoId, string subTaskId)
     {
         var removed = await _toDoService.RemoveSubTaskAsync(toDoId, subTaskId);
         if (!removed)
         {
             return NotFound();
         }
         return NoContent();
     }

     [HttpPost("{toDoId}/subtasks/{subTaskId}/toggle-completion")]
     public async Task<IActionResult> ToggleSubTaskCompletionAsync(string toDoId, string subTaskId)
     {
         await _toDoService.ToggleSubTaskCompletionAsync(toDoId, subTaskId);
         return NoContent();
     } 
}