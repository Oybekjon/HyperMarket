using HyperMarket.Queries.Settings;
using HyperMarket.Queries.Tests.DataHelpers;
using HyperMarket.Queries.User.Hashing;
using HyperMarket.Queries.User.ValidatePassword;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests
{
    public abstract class UserDataFixture<TQuery, TResult> : BaseDataFixture<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        public long ExistingUserId { get; protected set; }
        public Guid ExistingUserIdentifier { get; protected set; }

        public long FullUserId { get; protected set; }
        public Guid FullUserIdentifier { get; protected set; }
        public string FullUserEmail { get; protected set; }
        public string FullUserPhone { get; protected set; }
        public string Password { get; }

        public string NewUserEmail { get; }
        public string NewUserPhone { get; }
        public string NewUserPassword { get; }
        public Guid NewUserIdentifier { get; }

        public long NonExistingUserId { get;  }
        public long NonExistingStoreId { get; }

        public ValidatePasswordQueryHandler PasswordValidator { get; }
        public HashingQueryHandler Hashing { get; }

        public UserDataFixture()
        {
            Password = "MyPassword123__";

            NewUserEmail = "newuser@example.com";
            NewUserPhone = "998999999999";
            NewUserPassword = "!q4FFa_@()()";
            NewUserIdentifier = Guid.NewGuid();

            PasswordValidator = new ValidatePasswordQueryHandler();
            Hashing = new HashingQueryHandler(new EncryptionSettings
            {
                HashingKey = "SomeKey123@@__"
            });

            NonExistingUserId = uint.MaxValue;
            NonExistingStoreId = uint.MaxValue;
        }

        protected override void InitDatabase()
        {
            ExistingUserIdentifier = Guid.NewGuid();
            FullUserIdentifier = Guid.NewGuid();
            FullUserEmail = "user@example.com";
            FullUserPhone = "998905553344";

            Context.AddUser(out var userId, ExistingUserIdentifier);

            var pwdResult = AsyncHelpers.RunSync(() => Hashing.Handle(new HashingQuery
            {
                PlainText = Password
            }));

            Context.AddUser(out var fullUserId, FullUserIdentifier, x =>
            {
                x.FirstName = "John";
                x.LastName = "Doe";
                x.Email = FullUserEmail;
                x.EmailConfirmed = true;
                x.PasswordHash = pwdResult.HashedText;
                x.PhoneNumber = FullUserPhone;
                x.PhoneNumberConfirmed = true;
            });

            ExistingUserId = userId;
            FullUserId = fullUserId;
        }
    }
}
