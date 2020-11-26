using HyperMarket.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.User.Elevate
{
    public class ElevateUserQueryHandler : BusinessLogicQueryHandler<ElevateUserQuery, ElevateUserResult>
    {
        private readonly RepositoryContextBase Context;
        
        public ElevateUserQueryHandler(RepositoryContextBase context)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
        }

        public override Task<ElevateUserResult> Handle(ElevateUserQuery input)
        {
            throw new NotImplementedException();
        }
    }
}
