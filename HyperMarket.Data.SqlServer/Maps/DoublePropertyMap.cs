using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class DoublePropertyMap : IEntityTypeConfiguration<DoubleProperty>
    {
        public void Configure(EntityTypeBuilder<DoubleProperty> builder)
        {
            builder.Property(t => t.Name)
                   .IsRequired()
                   .HasMaxLength(400);
        }
    }
}
