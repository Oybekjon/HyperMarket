using HyperMarket.Queries.Tests.DataHelpers;
using HyperMarket.Queries.Tests.Store;
using HyperMarket.Queries.UserManagement.GetList;
using HyperMarket.Queries.ViewModels.UserManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests.UserManagement.GetList
{
    public class GetUserListQueryHandlerFixture : UserPermissionQueryHandlerFixture<GetUserListQuery, GetUserListResult>
    {
        public string U1FirstName { get; }
        public string U1LastName { get; }
        public Guid U1UserIdentifier { get; }
        public string U1Email { get; }
        public string U1PhoneNumber { get; }
        public long U1UserId { get; private set; }

        public string U2FirstName { get; }
        public string U2LastName { get; }
        public Guid U2UserIdentifier { get; }
        public string U2Email { get; }
        public string U2PhoneNumber { get; }
        public long U2UserId { get; private set; }

        public string U3FirstName { get; }
        public string U3LastName { get; }
        public Guid U3UserIdentifier { get; }
        public string U3Email { get; }
        public string U3PhoneNumber { get; }
        public long U3UserId { get; private set; }

        public GetUserListQueryHandlerFixture()
        {
            U1FirstName = "Jack";
            U1LastName = "Nicholson";
            U1UserIdentifier = Guid.NewGuid();
            U1Email = "jacknicholson@example.com";
            U1PhoneNumber = "998909931111";

            U2FirstName = "Rutger";
            U2LastName = "Hauer";
            U2UserIdentifier = Guid.NewGuid();
            U2Email = "rutgerhauer@example.com";
            U2PhoneNumber = "998909931112";

            U3FirstName = "Nicolas";
            U3LastName = "Cage";
            U3UserIdentifier = Guid.NewGuid();
            U3Email = "nicolascage@example.com";
            U3PhoneNumber = "998909931113";
        }

        protected override void InitDatabase()
        {
            base.InitDatabase();

            U1UserId = SaveUser(
                U1FirstName,
                U1LastName,
                U1UserIdentifier,
                U1Email,
                U1PhoneNumber
            );

            U2UserId = SaveUser(
                U2FirstName,
                U2LastName,
                U2UserIdentifier,
                U2Email,
                U2PhoneNumber
            );

            U3UserId = SaveUser(
                U3FirstName,
                U3LastName,
                U3UserIdentifier,
                U3Email,
                U3PhoneNumber
            );
        }

        private long SaveUser(
            string firstName,
            string lastName,
            Guid userIdentifier,
            string email,
            string phone)
        {

            Context.AddUser(out var userId, userIdentifier, x =>
            {
                x.FirstName = firstName;
                x.LastName = lastName;
                x.Email = email;
                x.EmailConfirmed = true;
                x.PhoneNumber = phone;
                x.PhoneNumberConfirmed = true;
            });

            return userId;
        }
    }
}
