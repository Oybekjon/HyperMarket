using HyperMarket.Data;
using HyperMarket.DomainObjects;
using HyperMarket.Queries.User.SendValidationEmail;
using HyperMarket.Queries.UserManagement.Create.Validation;
using HyperMarket.Queries.Validation;
using System;
using System.Linq;
using System.Threading.Tasks;
using HMStore = HyperMarket.DomainObjects.Store;
using HMUser = HyperMarket.DomainObjects.User;

namespace HyperMarket.Queries.UserManagement.Create
{
    public class CreateUserQueryHandler : BusinessLogicQueryHandler<CreateUserQuery, CreateUserResult>
    {
        private readonly RepositoryContextBase Context;
        private readonly IQueryHandler<SendValidationQuery, SendValidationResult> EmailSender;
        public CreateUserQueryHandler(
            RepositoryContextBase context,
            IQueryHandler<SendValidationQuery, SendValidationResult> emailSender
        )
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
            EmailSender = emailSender ?? throw ErrorHelper.ArgNull(nameof(emailSender));
        }

        public override async Task<CreateUserResult> Handle(CreateUserQuery input)
        {
            var validator = new CreateUserQueryValidator();
            validator.ValidateObject(input);
            var repo = Context.GetRepository<HMUser>();
            var newUser = new HMUser();

            if (input.StoreId.HasValue)
            {
                var storeRepo = Context.GetRepository<HMStore>();
                var storeExists = storeRepo
                    .Where(x => x.StoreId == input.StoreId)
                    .Any();

                if (!storeExists)
                {
                    throw ErrorHelper.NotFound("No such store");
                }
            }

            if (!string.IsNullOrWhiteSpace(input.Email))
            {
                var emailDuplicate = repo.Where(x => x.Email == input.Email).Any();
                if (emailDuplicate)
                {
                    throw ErrorHelper.Duplicate("Such email already exists");
                }

                newUser.Email = input.Email;
                newUser.EmailValidationCode = Guid.NewGuid().ToString();
            }

            if (!string.IsNullOrWhiteSpace(input.PhoneNumber))
            {
                var normalizedPhone = input.PhoneNumber.NormalizePhone();
                var phoneExists = repo.Where(x => x.PhoneNumber == normalizedPhone).Any();
                if (phoneExists)
                {
                    throw ErrorHelper.Duplicate("This phone already exists");
                }

                newUser.PhoneNumber = normalizedPhone;
            }

            newUser.FirstName = input.FirstName;
            newUser.LastName = input.LastName;
            newUser.UserIdentifier = input.UserIdentifier;
            newUser.DateCreated = DateTime.UtcNow;
            newUser.LastActivity = DateTime.UtcNow;
            newUser.IsActive = true;

            if (input.StoreId.HasValue)
            {
                newUser.UserStores.Add(new UserStore
                {
                    StoreId = input.StoreId.Value
                });
            }

            repo.Store(newUser);
            repo.SaveChanges();

            if (!string.IsNullOrEmpty(newUser.Email))
                await EmailSender.Handle(new SendValidationQuery
                {
                    Email = newUser.Email,
                    ValidationCode = newUser.EmailValidationCode
                });

            return new CreateUserResult();
        }
    }
}
