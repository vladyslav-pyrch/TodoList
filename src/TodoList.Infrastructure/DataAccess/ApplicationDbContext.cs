using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoList.Infrastructure.DataAccess.Models;

namespace TodoList.Infrastructure.DataAccess;

public class ApplicationDbContext : DbContext
{
    internal DbSet<TodoDbModel> Todos { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("Data Source=TodoList.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(GetType())!);
    }
}