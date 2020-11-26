using HyperMarket.Data;
using HyperMarket.DomainObjects;
using HyperMarket.Queries.UserManagement.AssignPermissions.Validation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMUser = HyperMarket.DomainObjects.User;

namespace HyperMarket.Queries.UserManagement.AssignPermissions
{
    public class AssignPermissionQueryHandler : BusinessLogicQueryHandler<AssignPermissionQuery, AssignPermissionResult>
    {
        private readonly RepositoryContextBase Context;
        public AssignPermissionQueryHandler(RepositoryContextBase context)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
        }

        public override Task<AssignPermissionResult> Handle(AssignPermissionQuery input)
        {
            var validator = new AssignPermissionQueryValidator();
            validator.ValidateObject(input);
            var userRepo = Context.GetRepository<HMUser>();
            var user = userRepo
                .Include(x => x.UserPermissions)
                .FirstOrDefault(x => x.UserId == input.UserId);

            if (user == null)
            {
                throw ErrorHelper.NotFound("No such user");
            }

            if (!user.IsFullUser())
            {
                throw ErrorHelper.NotAllowed("Only full users are allowed to have permissions");
            }

            var toDelete = user.UserPermissions
                .Where(x => !input.Permissions.Contains(x.Permission))
                .ToList();
            var existingPermissions = user.UserPermissions.Select(x => x.Permission).ToList();
            var toAdd = input.Permissions
                .Where(x => !existingPermissions.Contains(x))
                .Select(x => new UserPermission
                {
                    UserId = input.UserId,
                    Permission = x
                })
                .ToList();

            user.UserPermissions.RemoveAll(x => toDelete.Contains(x));
            user.UserPermissions.AddRange(toAdd);
            userRepo.SaveChanges();
            return Task.FromResult(new AssignPermissionResult());
        }
    }
}
