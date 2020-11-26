using HyperMarket.Data;
using HyperMarket.DomainObjects;
using HyperMarket.Queries.Store.Validation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Store.RemoveUserPermission
{
    public class RemoveUserPermissionQueryHandler : BusinessLogicQueryHandler<RemoveUserPermissionQuery, RemoveUserPermissionResult>
    {
        private readonly RepositoryContextBase Context;

        public RemoveUserPermissionQueryHandler(RepositoryContextBase context)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
        }

        public override Task<RemoveUserPermissionResult> Handle(RemoveUserPermissionQuery input)
        {
            var validator = new UserPermissionQueryValidation();
            validator.ValidateObject(input);

            var repo = Context.GetRepository<UserStorePermission>();
            var objectsToRemove = repo
                .Where(x =>
                    x.StoreId == input.StoreId &&
                    x.UserId == input.UserId &&
                    input.Permissions.Contains(x.Permission)
                )
                .ToList();

            foreach (var item in objectsToRemove)
            {
                repo.Delete(item);
            }

            repo.SaveChanges();

            return Task.FromResult(new RemoveUserPermissionResult());
        }
    }
}
