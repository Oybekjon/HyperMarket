using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HyperMarket.Queries.Store.AddLogo
{
    public class AddLogoQuery : IQuery<AddLogoResult>
    {
        public Stream Image { get; set; }
        public long StoreId { get; set; }
    }
}
