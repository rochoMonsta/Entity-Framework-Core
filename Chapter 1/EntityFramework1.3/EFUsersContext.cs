using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EFConnectToDB
{
    public partial class EFUsersContext : DbContext
    {
        public EFUsersContext() { }

        public EFUsersContext(DbContextOptions<EFUsersContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EFUsers;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasAnnotation("Relational:IsTableExcludedFromMigrations", false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
