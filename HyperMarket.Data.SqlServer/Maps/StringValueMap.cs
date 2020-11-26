using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class StringValueMap : IEntityTypeConfiguration<StringValue>
    {
        public void Configure(EntityTypeBuilder<StringValue> builder)
        {
            builder.Property(t => t.Value)
                   .IsRequired()
                   .HasMaxLength(400);
        }
    }
}
