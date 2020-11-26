using HyperMarket.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;
using HyperMarket.DomainObjects;
using HMUser = HyperMarket.DomainObjects.User;

namespace HyperMarket.Queries.Tests.DataHelpers
{
    public static class UserDataHelper
    {
        public static UserFluentDataHelper AddUser(this MainContext context, out long userId, Guid? identifier = null, Action<HMUser> setupAction = null)
        {
            var user = new HMUser
            {
                UserIdentifier = identifier ?? Guid.NewGuid(),
                DateCreated = DateTime.UtcNow,
                LastActivity = DateTime.UtcNow
            };
            setupAction?.Invoke(user);
            context.Users.Add(user);
            context.SaveChanges();
            userId = user.UserId;
            return new UserFluentDataHelper(context, user.UserId);
        }

        public class UserFluentDataHelper
        {
            public MainContext Context { get; }
            public long ParentId { get; }

            public UserFluentDataHelper(MainContext context, long parentId)
            {
                Context = context;
                ParentId = parentId;
            }
        }
    }
}
