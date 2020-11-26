using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.DomainObjects
{
    public class DoubleProperty
    {
        private ICollection<DoubleValue> _DoubleValues;
        private ICollection<ProductDoubleProperty> _ProductDoubleProperties;

        public long DoublePropertyId { get; set; }
        public string Name { get; set; }
        public bool IsEnum { get; set; }

        public ICollection<DoubleValue> DoubleValues
        {
            get => _DoubleValues ??= new List<DoubleValue>();
            set => _DoubleValues = value;
        }

        public ICollection<ProductDoubleProperty> ProductDoubleProperties
        {
            get => _ProductDoubleProperties ??= new List<ProductDoubleProperty>();
            set => _ProductDoubleProperties = value;
        }

    }
}
