using AspNetTasks.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetTasks.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated(); 
        }
    }
}
