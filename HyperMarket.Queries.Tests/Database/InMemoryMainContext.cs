using HyperMarket.Data.SqlServer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests.Database
{
    class InMemoryMainContext : MainContext
    {
        private SqliteConnection _masterConnection;
        public InMemoryMainContext(DbContextOptions<MainContext> options, string name)
            : base(options)
        {
            _masterConnection = new SqliteConnection($"Data Source={name};Mode=Memory;Cache=Shared");
            _masterConnection.Open();

            Database.EnsureCreated();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_masterConnection != null)
            {
                // Closing the master connection after which 
                // the database automatically gets erased
                _masterConnection.Dispose();
                _masterConnection = null;
            }
        }
    }
}
