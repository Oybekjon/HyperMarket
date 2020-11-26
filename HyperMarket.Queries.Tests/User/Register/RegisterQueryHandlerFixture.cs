using HyperMarket.Queries.Settings;
using HyperMarket.Queries.Tests.DataHelpers;
using HyperMarket.Queries.User.Hashing;
using HyperMarket.Queries.User.Register;
using HyperMarket.Queries.User.SendTextCode;
using HyperMarket.Queries.User.SendValidationEmail;
using HyperMarket.Queries.User.ValidatePassword;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests.User.Register
{
    public class RegisterQueryHandlerFixture : UserDataFixture<RegisterQuery, RegisterResult>
    {
        private Mock<IQueryHandler<SendValidationQuery, SendValidationResult>> _SendValidationMock;
        public Mock<IQueryHandler<SendValidationQuery, SendValidationResult>> SendValidationMock
        {
            get
            {
                if (_SendValidationMock == null)
                {
                    _SendValidationMock = new Mock<IQueryHandler<SendValidationQuery, SendValidationResult>>();
                    _SendValidationMock
                        .Setup(x => x.Handle(It.IsAny<SendValidationQuery>()))
                        .ReturnsAsync(new SendValidationResult());
                }
                return _SendValidationMock;
            }
        }
        private Mock<IQueryHandler<SendTextQuery, SendTextResult>> _SendTextMock;
        public Mock<IQueryHandler<SendTextQuery, SendTextResult>> SendTextMock
        {
            get
            {
                if (_SendTextMock == null)
                {
                    _SendTextMock = new Mock<IQueryHandler<SendTextQuery, SendTextResult>>();
                    _SendTextMock
                        .Setup(x => x.Handle(It.IsAny<SendTextQuery>()))
                        .ReturnsAsync(new SendTextResult());
                }
                return _SendTextMock;
            }
        }

        public RegisterQueryHandlerFixture()
        {
        }

        public RegisterQuery GetQuery(
            Guid? userIdentifier = null,
            string email = null,
            string phoneNumber = null,
            string firstName = null,
            string lastName = null,
            string password = null,
            string passwordConfirmation = null
        )
        {
            return new RegisterQuery
            {
                UserIdentifier = userIdentifier ?? NewUserIdentifier,
                Email = email ?? NewUserEmail,
                PhoneNumber = phoneNumber ?? NewUserPhone,
                FirstName = firstName ?? "Jane",
                LastName = lastName ?? "Doe",
                Password = password ?? NewUserPassword,
                PasswordConfirmation = passwordConfirmation ?? NewUserPassword
            };
        }

        public override BusinessLogicQueryHandler<RegisterQuery, RegisterResult> CreateService()
        {
            var repoContext = GetRepositoryContext();
            return new RegisterQueryHandler(
                repoContext,
                Hashing,
                PasswordValidator,
                SendTextMock.Object,
                SendValidationMock.Object
            );
        }
    }
}
