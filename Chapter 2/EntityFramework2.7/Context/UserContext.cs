using EntityFramework2._7.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework2._7.Context
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
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF2.7;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Age == 18 в тому випадку, якщо Age - не задано
            modelBuilder.Entity<User>().Property(u => u.Age).HasDefaultValue(18);

            //Передаємо в метод функцію SQL "GETDATE()" яка визначає дату додавання
            modelBuilder.Entity<User>()
                        .Property(u => u.CreatedAt)
                        .HasDefaultValueSql("GETDATE()");

            //Автогенерація значення FullName на основі значень Name та Surname
            modelBuilder.Entity<User>()
                        .Property(u => u.FullName)
                        .HasComputedColumnSql("[Name] + ' ' + [Surname]");

            //Заборона автоматичної генерації значення Id
            //modelBuilder.Entity<User>().Property(b => b.Id).ValueGeneratedNever();
        }
    }
}
