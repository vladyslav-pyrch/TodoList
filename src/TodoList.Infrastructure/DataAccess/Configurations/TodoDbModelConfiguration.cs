using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Infrastructure.DataAccess.Models;

namespace TodoList.Infrastructure.DataAccess.Configurations;

internal class TodoDbModelConfiguration : IEntityTypeConfiguration<TodoDbModel>
{
    public void Configure(EntityTypeBuilder<TodoDbModel> builder)
    {
        builder.ToTable("todos");
        
        builder.HasKey(model => model.Id);

        builder.Property(model => model.Id)
            .HasColumnName("id");
        builder.Property(model => model.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("name");
        builder.Property(model => model.Description)
            .HasMaxLength(500)
            .HasColumnName("description");
        builder.Property(model => model.IsDone)
            .IsRequired()
            .HasColumnName("is_done");
    }
}