using Microsoft.EntityFrameworkCore;


namespace ToDoListWeb.Entity
{
    public class GeneralTasksDbContext : DbContext
    {
        public DbSet<GeneralTasks> GeneralTasks { get; set; }

        private readonly IConfiguration _configuration;

        public GeneralTasksDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("GeneralTaskConnection"));

        }
    }
}
