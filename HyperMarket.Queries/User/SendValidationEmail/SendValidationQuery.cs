using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.User.SendValidationEmail
{
    public class SendValidationQuery : IQuery<SendValidationResult>
    {
        public string Email { get; set; }
        public string ValidationCode { get; set; }
    }
}
