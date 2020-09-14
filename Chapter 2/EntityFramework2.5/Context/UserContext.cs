using EntityFramework2._5.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework2._5.Context
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
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF2.5;Trusted_Connection=True;");
        }
        //Аналогічне перевизначення співставлення моделей можна зробити за допомогою Fluent API
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().ToTable("People"); //Перевизначаємо співставлення моделі(User) з таблицею(People)

        //    //Аналогічне перевизначення співставлення свойства з стовбцем таблиці через Fluent API - заміна [Column("user_id")]
        //    modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("user_id"); //Перевизначаємо назву стовбця замість (Id) в (user_id)
        //}
    }
}
