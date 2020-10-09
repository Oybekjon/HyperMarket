namespace HyperMarket.DomainObjects
{
    public class UserPermission
    {
        public long UserId { get; set; }
        public long PermissionId { get; set; }
        public User User { get; set; }
        public Permission Permission { get; set; }
    }
}