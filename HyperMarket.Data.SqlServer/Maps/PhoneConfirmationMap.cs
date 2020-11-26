using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class PhoneConfirmationMap : IEntityTypeConfiguration<PhoneConfirmation>
    {
        public void Configure(EntityTypeBuilder<PhoneConfirmation> builder)
        {
            builder.Property(t => t.Code)
                   .IsRequired()
                   .HasMaxLength(400);

            builder.Property(t => t.Phone)
                   .IsRequired()
                   .HasMaxLength(400);
        }
    }
}
