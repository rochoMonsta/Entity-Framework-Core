using EntityFramework5._9.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework5._9.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF5.9;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { UserID = 1, Name = "Roman", Surname = "Cholkan", Age = 20 },
                new User { UserID = 2, Name = "Emma", Surname = "Stone", Age = 19 },
                new User { UserID = 3, Name = "William", Surname = "Balck", Age = 29 },
                new User { UserID = 4, Name = "Robert", Surname = "Doms", Age = 61 }
            );

        }
    }
}
