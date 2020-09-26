using EntityFramework3._7.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework3._7.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF3.7;Trusted_Connection=True;");
        }
        // [Owned] - by Fluent API
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().OwnsOne(u => u.UserProfile);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().OwnsOne(u => u.UserProfile, p =>
            {
                p.OwnsOne(c => c.Name);
                p.OwnsOne(c => c.Surname);
                p.OwnsOne(c => c.Age);
            });
        }
    }
}
