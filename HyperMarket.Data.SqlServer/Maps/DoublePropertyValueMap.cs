using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class DoublePropertyValueMap : IEntityTypeConfiguration<DoublePropertyValue>
    {
        public void Configure(EntityTypeBuilder<DoublePropertyValue> builder)
        {
            
        }
    }
}
