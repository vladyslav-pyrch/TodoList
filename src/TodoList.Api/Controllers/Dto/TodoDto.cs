namespace TodoList.Controllers.Dto;

public class TodoDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public bool isDone { get; set; }
}