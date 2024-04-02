
using CloneKun.Models;
using Microsoft.EntityFrameworkCore;

namespace CloneKun.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Register>().HasNoKey();
        }

        public DbSet<Register> Register { get; set; }
    }
}
