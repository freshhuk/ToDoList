using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ToDoListWebDomain.Domain.Entity;

namespace ToDoListWebInfrastructure.Interfaces
{
    public interface IDataContext
    {
        DbSet<ToDoTask> ToDoTask { get; set; }
        List<ToDoTask> GetToDoTasks();
        Task AddAsync<TEntity>(TEntity entity) where TEntity : class;
        Task SaveChangesAsync();

    }
}
