using EntityFramework3._4.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework3._4.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public ApplicationContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF3.4;Trusted_Connection=True;");
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder
        //        .Entity<User>() // Головна модель
        //        .HasOne(u => u.Profile) // Свойство в якому буде зберігатись посилання на залежну модель
        //        .WithOne(p => p.User) // Свойство в якому буде зберігатись посилання на головну модель
        //        .HasForeignKey<UserProfile>(p => p.UserID); // Ключ по якому буде відбуватись звернення

        //    // Об'єднання моделей в одну таблицю
        //    modelBuilder.Entity<User>().ToTable("UsersFull");
        //    modelBuilder.Entity<UserProfile>().ToTable("UsersFull");
        //}
    }
}
