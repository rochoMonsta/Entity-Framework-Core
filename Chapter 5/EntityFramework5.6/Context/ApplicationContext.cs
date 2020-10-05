using EntityFramework5._6.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework5._6.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Company> Companies { get; set; }

        public ApplicationContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF5.6;Trusted_Connection=True;");
        }
    }
}
