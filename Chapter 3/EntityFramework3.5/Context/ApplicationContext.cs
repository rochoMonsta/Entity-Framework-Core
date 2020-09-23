using EntityFramework3._5.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework3._5.Context
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
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF3.5;Trusted_Connection=True;");
        }
    }
}
