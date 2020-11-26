using HyperMarket.Queries.Store.AssignUser;
using HyperMarket.Queries.Tests.DataHelpers;
using HyperMarket.Queries.User.Hashing;
using System;

namespace HyperMarket.Queries.Tests.Store.AssignUser
{
    public class AssignUserQueryHandlerFixture : UserPermissionQueryHandlerFixture<AssignUserQuery, AssignUserResult>
    {
        public string UnassignedUserEmail { get; }
        public string UnassignedUserPhone { get; }
        public string UnassignedUserPassword { get; }
        public Guid UnassignedUserIdentifier { get; }
        public long UnassignedUserId { get; private set; }

        public AssignUserQueryHandlerFixture()
        {
            UnassignedUserEmail = "newuser1@example.com";
            UnassignedUserPhone = "998999999991";
            UnassignedUserPassword = "!q4FFa_@()()";
            UnassignedUserIdentifier = Guid.NewGuid();
        }

        protected override void InitDatabase()
        {
            base.InitDatabase();

            var pwdResult = AsyncHelpers.RunSync(() => Hashing.Handle(new HashingQuery
            {
                PlainText = UnassignedUserPassword
            }));

            Context.AddUser(out var unassignedUserId, UnassignedUserIdentifier, x =>
            {
                x.FirstName = "John U";
                x.LastName = "Doe";
                x.Email = UnassignedUserEmail;
                x.EmailConfirmed = true;
                x.PasswordHash = pwdResult.HashedText;
                x.PhoneNumber = UnassignedUserPhone;
                x.PhoneNumberConfirmed = true;
            });

            UnassignedUserId = unassignedUserId;
        }
    }
}
