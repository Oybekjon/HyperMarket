using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(t => t.User)
                   .WithMany(t => t.Orders)
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.DeliveryUser)
                   .WithMany(t => t.OrdersToDeliver)
                   .HasForeignKey(t => t.DeliveryUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
