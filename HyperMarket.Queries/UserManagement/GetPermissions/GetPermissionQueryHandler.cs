using HyperMarket.Data;
using HyperMarket.Queries.UserManagement.GetPermissions.Validation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;
using HMUser = HyperMarket.DomainObjects.User;

namespace HyperMarket.Queries.UserManagement.GetPermissions
{
    public class GetPermissionQueryHandler : BusinessLogicQueryHandler<GetPermissionQuery, GetPermissionResult>
    {
        private readonly RepositoryContextBase Context;
        public GetPermissionQueryHandler(RepositoryContextBase context)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
        }

        public override Task<GetPermissionResult> Handle(GetPermissionQuery input)
        {
            var validator = new GetPermissionQueryValidator();
            validator.ValidateObject(input);
            var repo = Context.GetRepository<HMUser>();
            var user = repo
                .Include(x => x.UserPermissions)
                .FirstOrDefault(x => x.UserId == input.UserId);

            if (user == null)
            {
                throw ErrorHelper.NotFound("No such user");
            }

            var permissions = user.UserPermissions
                .Select(x => x.Permission)
                .ToList()
                .ToImmutableArray();

            return Task.FromResult(new GetPermissionResult(permissions));
        }
    }
}
