using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.DomainObjects
{
    public class Order : DatedEntity
    {
        private ICollection<OrderItem> _OrderItems;
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public long? DeliveryUserId { get; set; }
        public long? AddressId { get; set; }
        public DateTime? DateProcessed { get; set; }
        public DateTime? DateDelivered { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public User User { get; set; }
        public User DeliveryUser { get; set; }
        public decimal TotalSum { get; set; }
        public decimal Discount { get; set; }

        public Address AddressToDeliver { get; set; }
        public ICollection<OrderItem> OrderItems
        {
            get => _OrderItems ??= new List<OrderItem>();
            set => _OrderItems = value;
        }

    }
}
