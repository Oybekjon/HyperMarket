using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class ProductCategoryMap : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.Property(t => t.Name)
                   .IsRequired()
                   .HasMaxLength(400);

            builder.HasOne(t => t.ParentProductCategory)
                   .WithMany(t => t.Children)
                   .HasForeignKey(t => t.ParentProductCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
