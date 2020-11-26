using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.User.SendValidationEmail
{
    public class SendValidationQueryHandler : BusinessLogicQueryHandler<SendValidationQuery, SendValidationResult>
    {
        public override Task<SendValidationResult> Handle(SendValidationQuery input)
        {
            return Task.FromResult(new SendValidationResult());
        }
    }
}
