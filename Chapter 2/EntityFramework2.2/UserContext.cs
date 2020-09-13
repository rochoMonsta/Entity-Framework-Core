using Microsoft.EntityFrameworkCore;

namespace EFMigration
{
    class UserContext :DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EFMigrationDB;Trusted_Connection=True;");
        }
    }
}
