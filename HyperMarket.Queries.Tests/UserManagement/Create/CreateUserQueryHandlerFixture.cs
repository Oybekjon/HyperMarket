using HyperMarket.Queries.Tests.DataHelpers;
using HyperMarket.Queries.Tests.User;
using HyperMarket.Queries.UserManagement.Create;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests.UserManagement.Create
{
    public class CreateUserQueryHandlerFixture : UserDataFixture<CreateUserQuery, CreateUserResult>
    {
        public string NewUserFirstName { get; }
        public long StoreId { get; private set; }
        public CreateUserQueryHandlerFixture()
        {
            NewUserFirstName = Guid.NewGuid().ToString();
        }
        public CreateUserQuery GetQuery(
            string email = null,
            string phoneNumber = null,
            Guid?  userIdentifier = null,
            string firstName = null,
            string lastName = null,
            long? storeId = null
        )
        {
            return new CreateUserQuery { 
                Email = email ?? NewUserEmail,
                PhoneNumber = phoneNumber ?? NewUserPhone,
                UserIdentifier = userIdentifier ?? NewUserIdentifier,
                FirstName = firstName ?? NewUserFirstName,
                LastName = lastName,
                StoreId = storeId ?? StoreId
            };
        }

        protected override void InitDatabase()
        {
            base.InitDatabase();

            Context.AddStore(out var storeId, x=> {
                x.DisplayName =
                x.Description =
                    x.Name;
            });

            StoreId = storeId;
        }
    }
}
