using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class ProductDoublePropertyMap : IEntityTypeConfiguration<ProductDoubleProperty>
    {
        public void Configure(EntityTypeBuilder<ProductDoubleProperty> builder)
        {
            builder.HasKey(t => new { 
                t.ProductId,
                t.DoublePropertyId
            });

            builder.HasOne(t => t.Product)
                   .WithMany(t => t.ProductDoubleProperties)
                   .HasForeignKey(t => t.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.DoubleProperty)
                   .WithMany(t => t.ProductDoubleProperties)
                   .HasForeignKey(t => t.DoublePropertyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
