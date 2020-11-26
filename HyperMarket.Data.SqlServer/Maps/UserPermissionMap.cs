using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class UserPermissionMap : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.HasKey(t => new
            {
                t.UserId,
                t.Permission
            });

            builder.Property(t => t.Permission)
                   .IsRequired()
                   .HasMaxLength(400);
        }
    }
}
