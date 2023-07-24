using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ToDoListWeb.Entity;

namespace ToDoListWeb.Interfaces
{
    public interface IDataContext
    {
        DbSet<ToDoTask> ToDoTask { get; set; }
        Task AddAsync<TEntity>(TEntity entity) where TEntity : class;
        Task SaveChangesAsync();

    }
}
