using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.User.Hashing
{
    public class HashingQuery : IQuery<HashingResult>
    {
        public string PlainText { get; set; }
    }
}
