using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class StringPropertyValueMap : IEntityTypeConfiguration<StringPropertyValue>
    {
        public void Configure(EntityTypeBuilder<StringPropertyValue> builder)
        {
            
        }
    }
}
