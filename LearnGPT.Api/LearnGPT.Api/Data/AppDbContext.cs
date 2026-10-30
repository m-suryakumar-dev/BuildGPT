using LearnGPT.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnGPT.Api.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserCredential)
                .WithOne(c => c.User)
                .HasForeignKey<UserCredential>(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();    
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
    }
}
