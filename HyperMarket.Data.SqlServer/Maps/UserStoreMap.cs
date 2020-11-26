using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class UserStoreMap : IEntityTypeConfiguration<UserStore>
    {
        public void Configure(EntityTypeBuilder<UserStore> builder)
        {
            builder.HasKey(t => new { 
                t.UserId,
                t.StoreId
            });

            builder.HasOne(t => t.User)
                   .WithMany(t => t.UserStores)
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Store)
                   .WithMany(t => t.UserStores)
                   .HasForeignKey(t => t.StoreId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
