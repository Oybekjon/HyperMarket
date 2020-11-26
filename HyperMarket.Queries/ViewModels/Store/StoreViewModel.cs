using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.ViewModels.Store
{
    public class StoreViewModel
    {
        public long StoreId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
