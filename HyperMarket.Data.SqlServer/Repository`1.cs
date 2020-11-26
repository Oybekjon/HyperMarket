using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace HyperMarket.Data.SqlServer
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext Context;
        public Repository(DbContext context)
        {
            Guard.NotNull(context, "context");
            Context = context;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }

        public IQueryable<T> Include<TNavigationProperty>(Expression<Func<T, TNavigationProperty>> property)
        {
            return Context.Set<T>().Include(property);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().FirstOrDefault(predicate);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Count(predicate);
        }

        public int Count()
        {
            return Context.Set<T>().Count();
        }

        public int CountRaw(string query)
        {
            throw ErrorHelper.NotSupported();
        }

        public IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public void Store(T value)
        {
            Guard.NotNull(value, "value");
            if (Context.Entry(value).State == EntityState.Detached)
                Context.Set<T>().Add(value);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public void Delete(T value)
        {
            Guard.NotNull(value, "value");
            if (Context.Entry(value).State != EntityState.Deleted)
                Context.Set<T>().Remove(value);
        }

        public T GetById(object id)
        {
            throw ErrorHelper.NotSupported("This operation is not supported in this implementation of the repository");
        }

        public ICollection<T> RawQuery(String query)
        {
            throw ErrorHelper.NotSupported();
        }
    }
}
