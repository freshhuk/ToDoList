using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebInfrastructure.Interfaces;


namespace ToDoListWebInfrastructure.Context
{
    public class TaskDbContex : DbContext, IDataContext<ToDoTask>
    {
        public DbSet<ToDoTask> ToDoTask { get; set; }

        private readonly IConfiguration _configuration;

        public TaskDbContex(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //for configuration sql connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

        }
        #region Realization IDataContext
        public async Task AddAsync(ToDoTask item)
        {
            await ToDoTask.AddAsync(item);
        }

        public void Delete(int id)
        {
            var task = ToDoTask.Find(id);
            if (task != null)
                ToDoTask.Remove(task);
        }

        public ToDoTask Get(int id)
        {
            var task = ToDoTask.SingleOrDefault(t => t.Id == id);
            if (task == null)
            {
                // Можно выбросить исключение или выполнить другие действия по обработке
                throw new InvalidOperationException("Task not found.");
            }
            return task;
        }

        public IEnumerable<ToDoTask> GetAll()
        {
            return ToDoTask.OrderBy(t => t.Id);
        }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        public async Task Update(ToDoTask item)
        {
            var existingTask = await ToDoTask.FindAsync(item.Id);
            if (existingTask != null)
            {
                // Производим обновление полей существующей задачи
                Entry(existingTask).CurrentValues.SetValues(item);

            }
        }
        #endregion
        

    }
}
