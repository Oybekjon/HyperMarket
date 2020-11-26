using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.DomainObjects
{
    public class Product
    {
        private ICollection<ProductStock> _ProductStocks;
        private ICollection<ProductStringProperty> _ProductStringProperties;
        private ICollection<ProductDoubleProperty> _ProductDoubleProperties;
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long ProductCategoryId { get; set; }
        public long? ManufacturerId { get; set; }
        public bool Published { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public ICollection<ProductStock> ProductStocks
        {
            get => _ProductStocks ??= new List<ProductStock>();
            set => _ProductStocks = value;
        }

        public ICollection<ProductStringProperty> ProductStringProperties
        {
            get => _ProductStringProperties ??= new List<ProductStringProperty>();
            set => _ProductStringProperties = value;
        }

        public ICollection<ProductDoubleProperty> ProductDoubleProperties
        {
            get => _ProductDoubleProperties ??= new List<ProductDoubleProperty>();
            set => _ProductDoubleProperties = value;
        }

        public ProductCategory ProductCategory { get; set; }
    }
}
