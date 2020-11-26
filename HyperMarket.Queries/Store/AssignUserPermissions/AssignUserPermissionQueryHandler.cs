using HyperMarket.Data;
using HyperMarket.Queries.Store.Validation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HMUser = HyperMarket.DomainObjects.User;
using HMStore = HyperMarket.DomainObjects.Store;
using HyperMarket.DomainObjects;
using System.Linq;

namespace HyperMarket.Queries.Store.AssignUserPermissions
{
    public class AssignUserPermissionQueryHandler : BusinessLogicQueryHandler<AssignUserPermissionQuery, AssignUserPermissionResult>
    {
        private readonly RepositoryContextBase Context;
        public AssignUserPermissionQueryHandler(RepositoryContextBase context)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
        }

        public override Task<AssignUserPermissionResult> Handle(AssignUserPermissionQuery input)
        {
            var validator = new UserPermissionQueryValidation();
            validator.ValidateObject(input);

            var userStoreRepo = Context.GetRepository<UserStore>();
            var target = userStoreRepo
                .Include(x => x.UserStorePermissions)
                .FirstOrDefault(x => x.UserId == input.UserId && x.StoreId == input.StoreId);

            if (target == null)
            {
                throw ErrorHelper.NotFound("User store association is not found");
            }

            var existingPermissions = target.UserStorePermissions.Select(x => x.Permission).ToList();
            var newPermissions = input.Permissions.Where(x => !existingPermissions.Contains(x)).ToList();

            newPermissions.ForEach(x => target.UserStorePermissions.Add(new UserStorePermission
            {
                Permission = x
            }));

            userStoreRepo.SaveChanges();
            return Task.FromResult(new AssignUserPermissionResult());
        }
    }
}
