using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ToDoListWebDomain.Domain.Entity;

namespace ToDoListWebInfrastructure.Interfaces
{
    public interface IDataContext<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        Task AddAsync(T item);
        Task Update(T item);
        void Delete(int id);
        Task SaveChangesAsync();

    }
}
