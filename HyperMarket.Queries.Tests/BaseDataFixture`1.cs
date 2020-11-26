using HyperMarket.Data;
using HyperMarket.Data.SqlServer;
using HyperMarket.Queries.Tests.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace HyperMarket.Queries.Tests
{
    public abstract class BaseDataFixture<TQuery, TResult> : IDisposable where TQuery : IQuery<TResult>
    {
        private MainContext _Context;
        public MainContext Context
        {
            get
            {
                if (_Context == null)
                {
                    _Context = InMemoryDbHelper.CreateDbContext();
                    InitDatabase();
                }
                return _Context;
            }
        }

        protected abstract void InitDatabase();

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _Context?.Dispose();
                _Context = null;
            }

            _disposed = true;
        }

        public virtual BusinessLogicQueryHandler<TQuery, TResult> CreateService()
        {
            var repo = GetRepositoryContext();
            return QueriesVault.Instance.GetQueryHandler<TQuery, TResult>(repo);
        }

        protected RepositoryContextBase GetRepositoryContext()
        {
            return new SqlServerRepositoryContext(Context);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
