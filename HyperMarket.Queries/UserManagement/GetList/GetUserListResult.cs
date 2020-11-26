using HyperMarket.Queries.ViewModels;
using HyperMarket.Queries.ViewModels.UserManagement;
using System.Collections.Generic;

namespace HyperMarket.Queries.UserManagement.GetList
{
    public class GetUserListResult : ListViewModel<UserViewModel>
    {
        public GetUserListResult()
        {
        }

        public GetUserListResult(List<UserViewModel> data, int totalRecords, int limit, int offset)
            : base(data, totalRecords, limit, offset)
        {
        }
    }
}