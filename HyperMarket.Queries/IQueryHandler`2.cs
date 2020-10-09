using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery input);
    }
}
