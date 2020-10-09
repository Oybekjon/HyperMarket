using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Decorators
{
    public class LoggingDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> Decoratee;
        public LoggingDecorator(IQueryHandler<TQuery, TResult> decoratee)
        {
            Guard.NotNull(decoratee, "decoratee");
            Decoratee = decoratee;
        }
        public async Task<TResult> Handle(TQuery input)
        {
            return await Decoratee.Handle(input);
        }
    }
}
