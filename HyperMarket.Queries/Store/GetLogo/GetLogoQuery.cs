using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Store.GetLogo
{
    public class GetLogoQuery : IQuery<GetLogoResult>
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public long StoreId { get; set; }
    }
}
