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
            builder.Property(t => t.Email)
                .HasMaxLength(400);

            builder.HasIndex(t => t.UserIdentifier)
                   .IsUnique();
        }
    }
}
