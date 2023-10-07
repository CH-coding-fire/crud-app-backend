using fourthAPI.data;
using fourthAPI.DTOs;
using fourthAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace fourthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly CombinedDbContext _context;
        public TodoItemController(CombinedDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTodoItem(TodoItemCreationDTO request)
        {
            TodoItem todoItem = new TodoItem();
            todoItem.TodoGroupId = request.todoGroupId;
            todoItem.Name = request.name;
            todoItem.Status = request.status;
            todoItem.DueDate = request.dueDate.ToUniversalTime();
            todoItem.Description = request.description;
            todoItem.Priority = request.priority;
            todoItem.Tag = request.tag;
            todoItem.Created = DateTime.Now.ToUniversalTime();

            await _context.TodoItems.AddAsync(todoItem);
            await _context.SaveChangesAsync();


            TodoItem[] todoItems = await _context.TodoItems
              .Where(ti => ti.TodoGroupId == request.todoGroupId)
              .ToArrayAsync();


            return StatusCode(201, new { Message = "Create todo item successful", TodoItems = todoItems });
        }
        [HttpPut]
        public async Task<IActionResult> EditTodoItem(TodoItemEditDTO request)
        {
            TodoItem todoItem = await _context.TodoItems.FindAsync(request.id);

            if (todoItem == null)
            {
                return NotFound("Todo item not found");
            }

            todoItem.Name = request.name;
            todoItem.Status = request.status;
            todoItem.DueDate = request.dueDate.ToUniversalTime();
            todoItem.Description = request.description;
            todoItem.Priority = request.priority;
            todoItem.Tag = request.tag;

            // Save the changes
            _context.TodoItems.Update(todoItem);
            await _context.SaveChangesAsync();

            TodoItem[] todoItems = await _context.TodoItems
                .Where(ti => ti.TodoGroupId == todoItem.TodoGroupId)
                .ToArrayAsync();
            return StatusCode(200, new { Message = "Edit todo item successful", TodoItems = todoItems });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTodoItem(int todoItemId)
        {

            var todoItem = await _context.TodoItems.FindAsync(todoItemId);

            if (todoItem == null)
            {
                return NotFound(new { Message = "Todo item not found" });
            }

            // Remove the todo item from the database context
            _context.TodoItems.Remove(todoItem);

            // Save changes to database
            await _context.SaveChangesAsync();


            TodoItem[] todoItems = await _context.TodoItems
                .Where(ti => ti.TodoGroupId == todoItem.TodoGroupId)
                .ToArrayAsync();
            return StatusCode(200, new { Message = "Delete todo item successful", TodoItems = todoItems });
        }
    }
}
