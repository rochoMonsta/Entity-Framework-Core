using EntityFramework2._10.Configuration;
using EntityFramework2._10.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework2._10.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public ApplicationContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF2.10;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        }

        //Варіант коли в класі контексту створюються окермі методи для визначення конфігурацій:

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Product>(ProductConfigure);
        //    modelBuilder.Entity<Company>(CompanyConfigure);
        //}
        //// конфигурация для типа Product
        //public void ProductConfigure(EntityTypeBuilder<Product> builder)
        //{
        //    builder.ToTable("Mobiles").HasKey(p => p.Ident);
        //    builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
        //}
        //public void CompanyConfigure(EntityTypeBuilder<Company> builder)
        //{
        //    builder.ToTable("Manufacturers").Property(c => c.Name).IsRequired().HasMaxLength(30);
        //}
    }
}
