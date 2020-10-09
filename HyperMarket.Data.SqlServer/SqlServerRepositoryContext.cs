using System;
using System.Collections.Generic;
using System.Text;
using HyperMarket.Data;
using Microsoft.EntityFrameworkCore;

namespace HyperMarket.Data.SqlServer
{
    public class SqlServerRepositoryContext : RepositoryContextBase
    {
        private DbContext Context;
        public SqlServerRepositoryContext(DbContext context)
        {
            Guard.NotNull(context, "context");
            Context = context;
        }
        public override IRepository<T> GetRepository<T>()
        {
            return new Repository<T>(Context);
        }
        public override void InitializeDatabase()
        {
            Context.Database.EnsureCreated();
        }
    }
}
