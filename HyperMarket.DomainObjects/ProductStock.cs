using System.Collections.Generic;

namespace HyperMarket.DomainObjects
{
    public class ProductStock
    {
        public long ProductStockId { get; set; }
        public long StoreId { get; set; }
        public long ProductId { get; set; }
        public double CurrentStock { get; set; }
        public Product Product { get; set; }
        public Store Store { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}