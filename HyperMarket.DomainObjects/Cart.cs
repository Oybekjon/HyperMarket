using System.Collections.Generic;

namespace HyperMarket.DomainObjects
{
    public class Cart : DatedEntity
    {
        private ICollection<CartItem> _CartItems;
        public long CartId { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public ICollection<CartItem> CartItems
        {
            get => _CartItems ??= new List<CartItem>();
            set => _CartItems = value;
        }
    }
}