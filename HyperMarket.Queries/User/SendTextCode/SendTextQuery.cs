using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.User.SendTextCode
{
    public class SendTextQuery : IQuery<SendTextResult>
    {
        public string Phone { get; set; }
    }
}
