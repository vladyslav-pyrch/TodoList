using System.Reflection;
using Ishtar;
using Ishtar.DependencyInjection.Extensions;
using Ishtar.Extensions;
using TodoList.Core.Domain.Model.Todos;
using TodoList.Core.Services;
using TodoList.Infrastructure.DataAccess;
using TodoList.Infrastructure.DataAccess.Repositories;
using TodoList.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<TodoService>();

WebApplication app = await builder.Build();

app.Use<ExceptionHandlerMiddleware>();
app.UseEndpoints(Assembly.GetAssembly(typeof(Program))!);
// app.UseRun(() => { }); //Doesn't do anything. Without it an unmapped route responds with NotFound.
await app.Start();
