using EntityFramework3._1.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework3._1.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF3.1;Trusted_Connection=True;");
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasOne(p => p.Company)
        //        .WithMany(t => t.Users)
        //        .HasForeignKey(p => p.CompanyID);
        //}
    }
}
