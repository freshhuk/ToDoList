using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToDoListWeb.Models;

namespace ToDoListWeb.Entity
{
    public class TaskDbContex : DbContext
    {
        public DbSet<ToDoTask> ToDoTask { get; set; }
        

        private readonly IConfiguration _configuration;

        public TaskDbContex(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

        }
    }
}
