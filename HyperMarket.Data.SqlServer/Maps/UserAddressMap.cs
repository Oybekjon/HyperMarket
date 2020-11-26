using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class UserAddressMap : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.HasKey(t => new
            {
                t.UserId,
                t.AddressId
            });

            builder.HasOne(t => t.User)
                   .WithMany(t => t.Addresses)
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Address)
                   .WithMany(t => t.Users)
                   .HasForeignKey(t => t.AddressId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
