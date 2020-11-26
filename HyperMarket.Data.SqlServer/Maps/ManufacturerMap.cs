using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class ManufacturerMap : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder.Property(t => t.Name)
                   .IsRequired()
                   .HasMaxLength(400);
        }
    }
}
