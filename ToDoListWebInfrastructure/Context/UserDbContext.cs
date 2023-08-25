using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToDoListWebDomain.Domain.Models;

namespace ToDoListWebInfrastructure.Context
{
    public class UserDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }

        private readonly IConfiguration _configuration;

        public UserDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
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
