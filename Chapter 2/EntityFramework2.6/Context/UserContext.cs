using EntityFramework2._6.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework2._6.Context
{
    class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF2.6;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserNumber).HasName("UserId"); //Заміна [Key] через Fluent API

            //Створення составного ключа за допомогою Fluent API та задання альтернативної назив.
            //modelBuilder.Entity<User>().HasKey(u => new { u.PassportSeria, u.PassportNumber, u.UserNumber }).HasName("UserId");

            //Задаємо альтернативний ключ (Значення цього ключа також має бути унікальним, але це не первинний ключ).
            //modelBuilder.Entity<User>().HasAlternateKey(u => u.PassportNumber);

            //Створення составного алтернативного ключа
            modelBuilder.Entity<User>().HasAlternateKey(u => new { u.PhoneNumber, u.PassportNumber });

            //Створення індексу за допомогою HasIndex та вказування, що цей індекс є унікальним IsUnique (не повторюється)
            //modelBuilder.Entity<User>().HasIndex(u => u.PassportNumber).IsUnique();

            //Створення індексів для декількох свойств.
            modelBuilder.Entity<User>().HasIndex(u => new { u.PhoneNumber, u.PassportNumber }).IsUnique();
        }
    }
}
