using Ishtar;
using Ishtar.Abstractions;
using Microsoft.FSharp.Core;
using TodoList.Controllers.Dto;
using TodoList.Core.Domain.Model.Todos;
using TodoList.Core.Services;

namespace TodoList.Controllers;

[ApiController("/todo")]
public class TodoController(TodoService todoService) : ControllerBase
{
    [HttpRoute("/add", "POST")]
    public async Task<IActionResult> Add(NewTodoDto todoDto)
    {
        ErrorCollection errors = new ErrorCollection()
            .AddIf("Name is empty or null", () => string.IsNullOrWhiteSpace(todoDto.Name))
            .AddIf("Name may not be longer then 50 characters", () => todoDto.Name.Length > 50)
            .AddIf("Description may not be longer then 500 characters", () => todoDto.Description is { Length: > 500 });

        if (!errors.IsEmpty())
            return Result(HttpStatusCode.BadRequest400, errors);

        TodoId id = await todoService.Add(todoDto.Name, todoDto.Description ?? FSharpOption<string>.None);

        return Result(HttpStatusCode.Created201, new { Id = id.Value });
    }

    [HttpRoute("/update", "PATCH")]
    public async Task<IActionResult> Update(UpdateTodoDto todoDto)
    {
        ErrorCollection errors = new ErrorCollection()
            .AddIf("Id may not be null", () => todoDto.Id is null)
            .AddIf($"There is no todo with such Id: {todoDto.Id}",
                async () => todoDto.Id != null && !await todoService.ThereExists(new TodoId(todoDto.Id.Value)))
            .AddIf("Name may not be longer then 50 characters", () => todoDto.Name is { Length: > 50 })
            .AddIf("Description may not be longer then 500 characters",
                () => todoDto.Description is { Length: > 500 });
        if (!errors.IsEmpty())
            return Result(HttpStatusCode.BadRequest400, errors);

        var id = new TodoId(todoDto.Id!.Value);

        if (todoDto.Name is not null) await todoService.UpdateName(id, todoDto.Name);
        if (todoDto.Description is not null) await todoService.UpdateDescription(id, todoDto.Description);

        return Result(HttpStatusCode.Ok200);
    }

    [HttpRoute("/get_all", "GET")]
    public async Task<IActionResult> GetAll()
    {
        List<Todo> todos = (await todoService.GetAll()).ToList();

        return Result(HttpStatusCode.Ok200, todos);
    }

    [HttpRoute("/mark_done", "PUT")]
    public async Task<IActionResult> MarkDone(IHttpRequest request)
    {
        if (request.Query.TryGetValue("id", out string? idString))
        {
            var id = Convert.ToInt32(idString);
            
            ErrorCollection errors = new ErrorCollection()
                .AddIf($"There is no todo with such Id: {id}",
                    async () => !await todoService.ThereExists(new TodoId(id)));
            if (!errors.IsEmpty()) return Result(HttpStatusCode.BadRequest400, errors); 
            
            await todoService.MarkDone(new TodoId(id));
            return Result(HttpStatusCode.NoContent204);
        }
        else
        {
            var errors = new ErrorCollection { "No id was provided." };
            return Result(HttpStatusCode.BadRequest400, errors);
        }
    }
    
    [HttpRoute("/mark_undone", "PUT")]
    public async Task<IActionResult> MarkUndone(IHttpRequest request)
    {
        if (request.Query.TryGetValue("id", out string? idString))
        {
            var id = Convert.ToInt32(idString);
            
            ErrorCollection errors = new ErrorCollection()
                .AddIf($"There is no todo with such Id: {id}",
                    async () => !await todoService.ThereExists(new TodoId(id)));
            if (!errors.IsEmpty()) return Result(HttpStatusCode.BadRequest400, errors); 
            
            await todoService.MarkUndone(new TodoId(id));
            return Result(HttpStatusCode.NoContent204);
        }
        else
        {
            var errors = new ErrorCollection { "No id was provided." };
            return Result(HttpStatusCode.BadRequest400, errors);
        }
    }

    [HttpRoute("/delete", "DELETE")]
    public async Task<IActionResult> Delete(IHttpRequest request)
    {
        if (request.Query.TryGetValue("id", out string? idString))
        {
            var id = Convert.ToInt32(idString);
            
            ErrorCollection errors = new ErrorCollection()
                .AddIf($"There is no todo with such Id: {id}",
                    async () => !await todoService.ThereExists(new TodoId(id)));
            if (!errors.IsEmpty()) return Result(HttpStatusCode.BadRequest400, errors); 
            
            await todoService.Delete(new TodoId(id));
            return Result(HttpStatusCode.Ok200);
        }
        else
        {
            var errors = new ErrorCollection { "No id was provided." };
            return Result(HttpStatusCode.BadRequest400, errors);
        }
    }

    [HttpRoute("/delete_all_done", "DELETE")]
    public async Task<IActionResult> DeleteAllDone()
    {
        await todoService.DeleteAllDone();
        return Result(HttpStatusCode.Ok200);
    }
}