namespace TodoList.Infrastructure.DataAccess.Models;

internal class TodoDbModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
    
    public bool IsDone { get; set; }
}