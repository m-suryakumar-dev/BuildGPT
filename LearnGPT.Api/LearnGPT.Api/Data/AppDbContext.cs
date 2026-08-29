using LearnGPT.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnGPT.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

    }
}
