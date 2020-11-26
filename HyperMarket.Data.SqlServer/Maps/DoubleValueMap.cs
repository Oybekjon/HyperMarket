using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class DoubleValueMap : IEntityTypeConfiguration<DoubleValue>
    {
        public void Configure(EntityTypeBuilder<DoubleValue> builder)
        {
            
        }
    }
}
