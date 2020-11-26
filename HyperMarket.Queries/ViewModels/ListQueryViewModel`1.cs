using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.ViewModels
{
    public class ListQueryViewModel<T> : IQuery<T>
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
    }
}
