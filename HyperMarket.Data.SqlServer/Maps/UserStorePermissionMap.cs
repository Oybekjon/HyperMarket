using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class UserStorePermissionMap : IEntityTypeConfiguration<UserStorePermission>
    {
        public void Configure(EntityTypeBuilder<UserStorePermission> builder)
        {
            builder.HasKey(t => new
            {
                t.UserId,
                t.StoreId,
                t.Permission
            });

            builder.HasOne(t => t.UserStore)
                   .WithMany(t => t.UserStorePermissions)
                   .HasForeignKey(t => new
                   {
                       t.UserId,
                       t.StoreId
                   })
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
