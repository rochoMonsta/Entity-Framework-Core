using EntityFramework5._8.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework5._8.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF5.8;Trusted_Connection=True;");
        }
    }
}
