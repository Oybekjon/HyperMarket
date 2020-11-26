using HyperMarket.Data;
using HyperMarket.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HyperMarket.DomainObjects;
using HMUser = HyperMarket.DomainObjects.User;
using System.Linq;
using HyperMarket.Queries.User.Hashing;
using HyperMarket.Queries.User.ValidatePassword;
using HyperMarket.Queries.User.Register.Validation;
using HyperMarket.Queries.Validation;
using HyperMarket.Queries.ApiManagers;
using HyperMarket.Queries.User.SendTextCode;
using HyperMarket.Queries.User.SendValidationEmail;

namespace HyperMarket.Queries.User.Register
{
    public class RegisterQueryHandler : BusinessLogicQueryHandler<RegisterQuery, RegisterResult>
    {
        private readonly RepositoryContextBase Context;
        private readonly IQueryHandler<HashingQuery, HashingResult> Hasher;
        private readonly IQueryHandler<ValidatePasswordQuery, ValidatePasswordResult> PasswordValidator;
        private readonly IQueryHandler<SendTextQuery, SendTextResult> TextSender;
        private readonly IQueryHandler<SendValidationQuery, SendValidationResult> EmailSender;

        public RegisterQueryHandler(
            RepositoryContextBase context,
            IQueryHandler<HashingQuery, HashingResult> hasher,
            IQueryHandler<ValidatePasswordQuery, ValidatePasswordResult> passwordValidator,
            IQueryHandler<SendTextQuery, SendTextResult> textSender,
            IQueryHandler<SendValidationQuery, SendValidationResult> emailSender
        )
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
            Hasher = hasher ?? throw ErrorHelper.ArgNull(nameof(hasher));
            PasswordValidator = passwordValidator ?? throw ErrorHelper.ArgNull(nameof(hasher));
            TextSender = textSender ?? throw ErrorHelper.ArgNull(nameof(textSender));
            EmailSender = emailSender ?? throw ErrorHelper.ArgNull(nameof(emailSender));
        }

        public override async Task<RegisterResult> Handle(RegisterQuery input)
        {
            Guard.NotNull(input, nameof(input));
            Guard.PropertyNotNull(input.UserIdentifier, nameof(input.UserIdentifier));
            var repo = Context.GetRepository<HMUser>();
            var newUser = new HMUser();
            var validator = new RegisterQueryValidator();
            validator.ValidateObject(input);

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

            if (!string.IsNullOrWhiteSpace(input.Password) || !string.IsNullOrWhiteSpace(input.PasswordConfirmation))
            {
                await PasswordValidator.Handle(new ValidatePasswordQuery
                {
                    Password = input.Password,
                    PasswordConfirmation = input.PasswordConfirmation
                });

                var hashingResult = await Hasher.Handle(new HashingQuery
                {
                    PlainText = input.Password
                });

                newUser.PasswordHash = hashingResult.HashedText;
            }

            newUser.FirstName = input.FirstName;
            newUser.LastName = input.LastName;
            newUser.UserIdentifier = input.UserIdentifier;
            newUser.DateCreated = DateTime.UtcNow;
            newUser.LastActivity = DateTime.UtcNow;
            newUser.IsActive = true;

            repo.Store(newUser);
            repo.SaveChanges();

            if (!string.IsNullOrEmpty(newUser.PhoneNumber))
                await TextSender.Handle(new SendTextQuery
                {
                    Phone = newUser.PhoneNumber
                });

            if (!string.IsNullOrEmpty(newUser.Email))
                await EmailSender.Handle(new SendValidationQuery { 
                    Email = newUser.Email,
                    ValidationCode = newUser.EmailValidationCode
                });

            return new RegisterResult
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                UserId = newUser.UserId
            };
        }
    }
}
