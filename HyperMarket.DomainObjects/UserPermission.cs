namespace HyperMarket.DomainObjects
{
    public class UserPermission
    {
        public string Permission { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}