using System.Collections.Generic;

namespace HyperMarket.DomainObjects
{
    public class ProductStock
    {
        private ICollection<CartItem> _CartItems;
        private ICollection<StringPropertyValue> _StringPropertyValues;
        private ICollection<DoublePropertyValue> _DoublePropertyValues;
        private ICollection<OrderItem> _OrderItems;

        public long ProductStockId { get; set; }
        public long StoreId { get; set; }
        public long ProductId { get; set; }
        public double CurrentStock { get; set; }
        public Product Product { get; set; }
        public Store Store { get; set; }

        public ICollection<CartItem> CartItems
        {
            get => _CartItems ??= new List<CartItem>();
            set => _CartItems = value;
        }

        public ICollection<StringPropertyValue> StringPropertyValues
        {
            get => _StringPropertyValues ??= new List<StringPropertyValue>();
            set => _StringPropertyValues = value;
        }

        public ICollection<DoublePropertyValue> DoublePropertyValues
        {
            get => _DoublePropertyValues ??= new List<DoublePropertyValue>();
            set => _DoublePropertyValues = value;
        }

        public ICollection<OrderItem> OrderItems
        {
            get => _OrderItems ??= new List<OrderItem>();
            set => _OrderItems = value;
        }
    }
}