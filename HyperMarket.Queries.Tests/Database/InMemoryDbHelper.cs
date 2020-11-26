using HyperMarket.Data.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests.Database
{
    internal static class InMemoryDbHelper
    {

        public static MainContext CreateDbContext()
        {
            var name = $"InMemory{Guid.NewGuid():N}";
            var result = new InMemoryMainContext(CreateOptions(name), name);

            return result;
        }

        private static DbContextOptions<MainContext> CreateOptions(string name)
        {
            var builder = new DbContextOptionsBuilder<MainContext>();
            builder.UseSqlite($"Data Source={name};Mode=Memory;Cache=Shared");

            return builder.Options;
        }
    }
}
