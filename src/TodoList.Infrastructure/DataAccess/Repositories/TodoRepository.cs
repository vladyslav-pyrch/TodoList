using Microsoft.EntityFrameworkCore;
using Microsoft.FSharp.Collections;
using TodoList.Core.Domain.Model.Todos;
using TodoList.Infrastructure.DataAccess.Models;

namespace TodoList.Infrastructure.DataAccess.Repositories;

public class TodoRepository(ApplicationDbContext dbContext) : ITodoRepository
{
    public TodoId NewId()
    {
        var todo = new TodoDbModel { Name = "Not Valid Name", Description = ""};
        
        dbContext.Todos.Add(todo);
        dbContext.SaveChanges();

        int id = todo.Id;

        dbContext.Todos.Remove(todo);
        dbContext.SaveChanges();

        return new TodoId(id);
    }

    public Task SaveChanges(CancellationToken token = default)
        => dbContext.SaveChangesAsync(token);

    public async Task<FSharpList<Todo>> GetAll()
    {
        List<Todo> todos = await dbContext.Todos.Select(
                model => new Todo(new TodoId(model.Id), model.Name, model.Description, model.IsDone))
            .ToListAsync();

        return ListModule.OfSeq(todos);
    }

    public async Task<Todo> GetById(TodoId id)
    {
        TodoDbModel model = await dbContext.Todos.AsNoTracking().SingleAsync(dbModel => dbModel.Id == id.Value);
        return new Todo(id, model.Name, model.Description, model.IsDone);
    }

    public Task Add(Todo todo)
    {
        var model = new TodoDbModel()
        {
            Id = todo.Id.Value,
            Name = todo.Name,
            Description = todo.Description,
            IsDone = todo.IsDone
        };

        return dbContext.Todos.AddAsync(model).AsTask();
    }

    public Task Update(Todo todo)
    {
        var model = new TodoDbModel()
        {
            Id = todo.Id.Value,
            Name = todo.Name,
            Description = todo.Description,
            IsDone = todo.IsDone
        };

        return Task.Run(() => dbContext.Todos.Update(model));
    }

    public async Task Remove(TodoId id)
    {
        dbContext.Todos.Remove(await dbContext.Todos.SingleAsync(model => model.Id == id.Value));
    }

    public async Task<FSharpList<Todo>> GetUndone()
    {
        List<Todo> doneTodos = await dbContext.Todos.Where(model => model.IsDone == false)
            .Select(model => new Todo(new TodoId(model.Id), model.Name, model.Description, model.IsDone))
            .ToListAsync();

        return ListModule.OfSeq(doneTodos);
    }

    public async Task<FSharpList<Todo>> GetDone()
    {
        List<Todo> doneTodos = await dbContext.Todos.Where(model => model.IsDone == true)
            .Select(model => new Todo(new TodoId(model.Id), model.Name, model.Description, model.IsDone))
            .ToListAsync();

        return ListModule.OfSeq(doneTodos);
    }

    public async Task<bool> ThereExists(TodoId id) => await dbContext.Todos.AsNoTracking()
        .FirstOrDefaultAsync(model => model.Id == id.Value) is not null;
}