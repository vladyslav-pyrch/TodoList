using System.Collections;

namespace TodoList.Controllers.Dto;

public class ErrorCollection : IEnumerable<string>
{
    private readonly List<string> _errors = new()
    {
        Capacity = 5
    };

    public List<string> Errors => _errors;

    public ErrorCollection AddIf(string message, Func<bool> condition)
    {
        if (condition())
            Add(message);
        return this;
    }

    public ErrorCollection AddIf(string message, Func<Task<bool>> condition)
    {
        Task<bool> result = condition();
        result.Wait();
        if (result.Result)
            Add(message);
        return this;
    }
    
    public bool IsEmpty() => _errors.Count == 0;

    public IEnumerator<string> GetEnumerator() => _errors.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_errors).GetEnumerator();

    public void Add(string item) => _errors.Add(item);
}