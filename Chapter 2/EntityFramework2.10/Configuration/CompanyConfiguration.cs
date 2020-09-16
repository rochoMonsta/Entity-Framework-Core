using EntityFramework2._10.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework2._10.Configuration
{
    class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Manufacturers").Property(c => c.Name).IsRequired().HasMaxLength(30);
        }
    }
}
