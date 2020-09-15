using EntityFramework2._8.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework2._8.Context
{
    class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF2.8;Trusted_Connection=True;");
        }
        // [Required] через Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Вказуємо, що свойство Name є NOT NULL
            modelBuilder.Entity<User>().Property(b => b.Name).IsRequired();

            //Вказуємо, що свойство Surname має максимальну довжину рядка 50.
            modelBuilder.Entity<User>().Property(b => b.Surname).HasMaxLength(50);
        }
    }
}
