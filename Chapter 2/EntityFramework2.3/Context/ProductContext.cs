using EntityFramework2._3.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework2._3.Context
{
    class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public ProductContext() 
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFShop;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Створення нової таблиці по заданій моделі
            modelBuilder.Entity<Country>();
            //modelBuilder.Ignore<Company>(); - створення таблиці по моделі Company не буде відбуватись
        }
    }
}
