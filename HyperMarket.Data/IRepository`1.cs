using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace HyperMarket.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<T> Include<TNavigationProperty>(Expression<Func<T, TNavigationProperty>> property);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
        int Count();
        int CountRaw(string query);
        IQueryable<T> GetAll();
        void Store(T value);
        int SaveChanges();
        void Delete(T value);
        T GetById(object id);
        ICollection<T> RawQuery(string query);
    }
}