using System;
using System.Collections.Generic;
using System.Text;
using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperMarket.Data.SqlServer.Maps
{
    internal class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(400);

            builder.HasAlternateKey(t => t.Email);
        }
    }
}
