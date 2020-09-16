using EntityFramework2._11.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework2._11.Context
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
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF2.11;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Ініціалізація таблиці User початковими значенями (одноразово при створені)
            modelBuilder.Entity<User>().HasData(
                new User[]
                {
                    new User() { UserID = 1, Name = "Roman", Surname = "Cholkan", Age = 20 },
                    new User() { UserID = 2, Name = "Mia", Surname = "Sorokotyaha", Age = 20 }
                });
        }
    }
}
