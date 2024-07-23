using Ishtar.Abstractions;

namespace TodoList.Middlewares;

public class ExceptionHandlerMiddleware : IMiddleware
{
    public IMiddleware Next { get; set; } = null!;
    
    public async Task Invoke(IHttpContext context)
    {
        try
        {
             await Next.Invoke(context);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            context.Response.StatusCode = HttpStatusCode.InternalServerError500;
            context.Response.Body = [];
            context.Response.Headers.Clear();
            context.Response.Version = HttpVersion.Version11;
        }
    }
}