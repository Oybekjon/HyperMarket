using HyperMarket.Data;
using HyperMarket.DomainObjects;
using HyperMarket.Queries.Store.AssignUser.Validation;
using HyperMarket.Queries.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMStore = HyperMarket.DomainObjects.Store;
using HMUser = HyperMarket.DomainObjects.User;

namespace HyperMarket.Queries.Store.AssignUser
{
    public class AssignUserQueryHandler : BusinessLogicQueryHandler<AssignUserQuery, AssignUserResult>
    {
        private readonly RepositoryContextBase Context;

        public AssignUserQueryHandler(RepositoryContextBase context)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
        }

        public override Task<AssignUserResult> Handle(AssignUserQuery input)
        {
            var validator = new AssignUserQueryValidator();
            validator.ValidateObject(input);

            var storeRepo = Context.GetRepository<HMStore>();
            var userRepo = Context.GetRepository<HMUser>();
            var userStoreRepo = Context.GetRepository<UserStore>();

            var userQuery = userRepo.Where(x => x.UserId == input.UserId)
                .Select(x => new
                {
                    Type = "User",
                    Exists = true,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    EmailConfirmed = x.EmailConfirmed
                });

            var storeQuery = storeRepo.Where(x => x.StoreId == input.StoreId)
                .Select(x => new
                {
                    Type = "Store",
                    Exists = true,
                    PhoneNumber = (string)null,
                    Email = (string)null,
                    EmailConfirmed = false
                });

            var userStoreQuery = userStoreRepo.Where(x => x.UserId == input.UserId && x.StoreId == input.StoreId)
                .Select(x => new
                {
                    Type = "UserStore",
                    Exists = true,
                    PhoneNumber = (string)null,
                    Email = (string)null,
                    EmailConfirmed = false
                });

            var materialized = userQuery.Union(storeQuery).Union(userStoreQuery).ToList();

            var text = new StringBuilder();
            var @throw = false;
            var storeExists = materialized.FirstOrDefault(x => x.Type == "Store");
            if (storeExists == null)
            {
                text.Append("Store not found").AppendLine();
                @throw = true;
            }
            var userExists = materialized.FirstOrDefault(x => x.Type == "User");
            if (userExists == null)
            {
                text.Append("User not found").AppendLine();
                @throw = true;
            }

            if (@throw)
            {
                throw ErrorHelper.NotFound(text.ToString());
            }

            if (
                (
                    string.IsNullOrWhiteSpace(userExists.Email) || 
                    !userExists.EmailConfirmed
                ) && string.IsNullOrWhiteSpace(userExists.PhoneNumber))
            {
                throw ErrorHelper.NotAllowed("Cannot add this user. It has no phone number and email. It should have at least one of them");
            }

            var associationExists = materialized.FirstOrDefault(x => x.Type == "UserStore");
            if (associationExists != null)
            {
                throw ErrorHelper.Duplicate("Association already exists");
            }

            var association = new UserStore
            {
                StoreId = input.StoreId,
                UserId = input.UserId
            };

            association.UserStorePermissions = input.Permissions.Select(x => new UserStorePermission
            {
                StoreId = input.StoreId,
                UserId = input.UserId,
                Permission = x
            }).ToList();

            userStoreRepo.Store(association);
            userStoreRepo.SaveChanges();
            return Task.FromResult(new AssignUserResult());
        }
    }
}
