using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebInfrastructure.Interfaces;


namespace ToDoListWebInfrastructure.Context
{
    public class TaskDbContex : DbContext, IDataContext
    {
        public DbSet<ToDoTask> ToDoTask { get; set; }

        private readonly IConfiguration _configuration;

        public TaskDbContex(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //метод реализации интрефейса для добовление сущностей в бд
        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await Set<TEntity>().AddAsync(entity);
        }
        //метод для получения наших данных
        public List<ToDoTask> GetToDoTasks()
        {
            return ToDoTask.OrderBy(t => t.Id).ToList();
        }
        //метод реализации интерфейса для сохранений данных в бд
        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

        }

    }
}
