using EntityFramework5._4.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework5._4.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF5.4;Trusted_Connection=True;");
        }
    }
}