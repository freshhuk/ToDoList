using Microsoft.EntityFrameworkCore;
namespace ToDoListWeb.Entity
{
    public class TaskDbContex : DbContext
    {
        public DbSet<ToDoTask> ToDoTask { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;
                  DataBase=Task_EntityCoreDb;
                  Trusted_Connection=True;"
                );
        }
    }
}
