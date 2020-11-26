using HyperMarket.Data;
using HyperMarket.DomainObjects;
using HyperMarket.Queries.UserManagement.GetList.Validation;
using HyperMarket.Queries.ViewModels.UserManagement;
using System.Linq;
using System.Threading.Tasks;
using HMUser = HyperMarket.DomainObjects.User;

namespace HyperMarket.Queries.UserManagement.GetList
{
    public class GetUserListQueryHandler : BusinessLogicQueryHandler<GetUserListQuery, GetUserListResult>
    {
        private readonly RepositoryContextBase Context;
        public GetUserListQueryHandler(RepositoryContextBase context)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
        }

        public override Task<GetUserListResult> Handle(GetUserListQuery input)
        {
            var validator = new GetUserListQueryValidator();
            validator.Validate(input);

            var repo = Context.GetRepository<HMUser>();
            var userQuery = repo.GetAll();

            if (input.StoreId.HasValue)
            {
                var userStoreRepo = Context.GetRepository<UserStore>();
                userQuery = userStoreRepo.Where(x => x.StoreId == input.StoreId).Select(x => x.User);
            }

            if (input.UserId.HasValue)
            {
                userQuery = userQuery.Where(x => x.UserId == input.UserId);
            }

            if (input.FullOnly)
            {
                userQuery = userQuery.Where(x =>
                    (x.PhoneNumber != null && x.PhoneNumber != "")
                    ||
                    (x.Email != null && x.Email != "" && x.EmailConfirmed)
                );
            }

            if (!string.IsNullOrEmpty(input.Email))
            {
                userQuery = userQuery.Where(x => x.Email == input.Email);
            }

            if (!string.IsNullOrEmpty(input.NameQuery))
            {
                userQuery = userQuery.Where(x => x.FirstName.Contains(input.NameQuery) || x.LastName.Contains(input.NameQuery));
            }

            var totalRecords = userQuery.Count();

            userQuery = userQuery.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);

            if (!input.Limit.HasValue)
            {
                input.Limit = ushort.MaxValue;
            }

            if (!input.Offset.HasValue)
            {
                input.Offset = 0;
            }

            if (input.Offset.Value > 0)
            {
                userQuery = userQuery.Skip(input.Offset.Value);
            }

            userQuery = userQuery.Take(input.Limit.Value);

            var materialized = userQuery.Select(x => new UserViewModel
            {
                UserId = x.UserId,
                UserIdentifier = x.UserIdentifier,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                EmailValidationCode = x.EmailValidationCode,
                EmailConfirmed = x.EmailConfirmed,
                PhoneNumber = x.PhoneNumber,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                LastActivity = x.LastActivity,
                IsActive = x.IsActive
            }).ToList();

            var result = new GetUserListResult(materialized, totalRecords, input.Limit.Value, input.Offset.Value); ;
            return Task.FromResult(result);
        }
    }
}
