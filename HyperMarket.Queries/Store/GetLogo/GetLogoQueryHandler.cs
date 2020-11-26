using HyperMarket.Queries.Store.GetLogo.Validation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Store.GetLogo
{
    public class GetLogoQueryHandler : BusinessLogicQueryHandler<GetLogoQuery, GetLogoResult>
    {
        public override Task<GetLogoResult> Handle(GetLogoQuery input)
        {
            var validator = new GetLogoQueryValidator();
            validator.ValidateObject(input);
            throw new NotImplementedException();
        }
    }
}
