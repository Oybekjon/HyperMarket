using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class StringPropertyMap : IEntityTypeConfiguration<StringProperty>
    {
        public void Configure(EntityTypeBuilder<StringProperty> builder)
        {
            builder.Property(t => t.Name)
                   .IsRequired()
                   .HasMaxLength(400);
        }
    }
}
