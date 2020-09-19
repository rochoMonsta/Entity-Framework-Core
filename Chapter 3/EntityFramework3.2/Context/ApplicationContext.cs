using EntityFramework3._2.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework3._2.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF3.2;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cascade: зависимая сущность удаляется вместе с главной
            // SetNull: свойство-внешний ключ в зависимой сущности получает значение null
            // Restrict: зависимая сущность никак не изменяется при удалении главной сущности

            modelBuilder.Entity<User>()
                .HasOne(p => p.Company)
                .WithMany(t => t.Users)
                .OnDelete(DeleteBehavior.Cascade); // Якщо користувач належить якійсь компанії, то при її видалені
                                                   // видалиться і користувач з таблиці користувачів
        }
    }
}
