using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.DomainObjects
{
    public class StringProperty
    {
        private ICollection<StringValue> _StringValues;
        private ICollection<ProductStringProperty> _ProductStringProperties;

        public long StringPropertyId { get; set; }
        public string Name { get; set; }
        public bool IsEnum { get; set; }

        public ICollection<StringValue> StringValues
        {
            get => _StringValues ??= new List<StringValue>();
            set => _StringValues = value;
        }

        public ICollection<ProductStringProperty> ProductStringProperties
        {
            get => _ProductStringProperties ??= new List<ProductStringProperty>();
            set => _ProductStringProperties = value;
        }
    }
}
