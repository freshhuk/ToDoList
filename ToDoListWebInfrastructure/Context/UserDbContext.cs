using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToDoListWebDomain.Domain.Entity;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebInfrastructure.Interfaces;

namespace ToDoListWebInfrastructure.Context
{
    public class UserDbContext : IdentityDbContext<User>, IDataContext<User>
    {
        public override DbSet<User> Users { get; set; }

        private readonly IConfiguration _configuration;

        public UserDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Realization IDataContext
        public async Task AddAsync(User item)
        {
            await Users.AddAsync(item);
        }

        public void Delete(int id)
        {
            var task = Users.Find(id);
            if (task != null)
                Users.Remove(task);
        }
        //потом доделать
        public User Get(int id)
        {
            var task = Users.SingleOrDefault(t => t.Id == id.ToString());
            if (task == null)
            {
                // Можно выбросить исключение или выполнить другие действия по обработке
                throw new InvalidOperationException("Task not found.");
            }
            return task;
        }
        

        public IEnumerable<User> GetAll()
        {
            return Users.OrderBy(t => t.Id);
        }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        public async Task Update(User item)
        {
            var existingTask = await Users.FindAsync(item.Id);
            if (existingTask != null)
            {
                // Производим обновление полей существующей задачи
                Entry(existingTask).CurrentValues.SetValues(item);

            }
        }
        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Включите тип IdentityUserClaim<string> в модель
            builder.Entity<IdentityUserClaim<string>>().ToTable("User_EntityCoreDb");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("UserConnection"));

        }
    }
}
