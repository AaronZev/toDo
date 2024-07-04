using Microsoft.EntityFrameworkCore;
using toDoList.Models.toDoList.Models;

namespace toDoList.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<W_USER> W_USER { get; set; }
        public DbSet<W_TASK> W_TASK { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
