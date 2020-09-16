using EntityFramework2._10.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework2._10.Configuration
{
    class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Mobiles").HasKey(p => p.ProductID);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
        }
    }
}
