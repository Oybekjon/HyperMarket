using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HyperMarket.Queries;

namespace HyperMarket.Web.ActionHelpers
{
    public class QueryManager
    {
        private readonly IServiceProvider Provider;
        public QueryManager(IServiceProvider provider)
        {
            Provider = provider;
        }

        public IQueryHandler<T, TResult> Resolve<T, TResult>() where T : IQuery<TResult>
        {
            return (IQueryHandler<T, TResult>)Provider.GetService(typeof(IQueryHandler<T, TResult>));
        }
    }
}
