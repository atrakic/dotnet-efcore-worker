using Microsoft.EntityFrameworkCore;

namespace WorkerServicePlusEFCore.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
