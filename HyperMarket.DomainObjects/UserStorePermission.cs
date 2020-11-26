namespace HyperMarket.DomainObjects
{
    public class UserStorePermission
    {
        public long UserId { get; set; }
        public long StoreId { get; set; }
        public string Permission{ get; set; }
        public UserStore UserStore { get; set; }
    }
}