using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.DomainObjects
{
    public class Product
    {
        private ICollection<ProductStock> _ProductStocks;

        public long ProductId { get; set; }
        public string Name { get; set; }
        public long ProductCategoryId { get; set; }
        public long? ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ICollection<ProductStock> ProductStocks
        {
            get => _ProductStocks ??= new List<ProductStock>();
            set => _ProductStocks = value;
        }
    }
}
