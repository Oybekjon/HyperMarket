using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class ProductStringPropertyMap : IEntityTypeConfiguration<ProductStringProperty>
    {
        public void Configure(EntityTypeBuilder<ProductStringProperty> builder)
        {
            builder.HasKey(t => new
            {
                t.ProductId,
                t.StringPropertyId
            });

            builder.HasOne(t => t.Product)
                   .WithMany(t => t.ProductStringProperties)
                   .HasForeignKey(t => t.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.StringProperty)
                   .WithMany(t => t.ProductStringProperties)
                   .HasForeignKey(t => t.StringPropertyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
