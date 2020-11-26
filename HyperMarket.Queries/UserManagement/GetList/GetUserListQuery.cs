using HyperMarket.Queries.ViewModels;

namespace HyperMarket.Queries.UserManagement.GetList
{
    public class GetUserListQuery : ListQueryViewModel<GetUserListResult>
    {
        public long? StoreId { get; set; }
        public bool FullOnly { get; set; }
        public string NameQuery { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public long? UserId { get; set; }
    }
}