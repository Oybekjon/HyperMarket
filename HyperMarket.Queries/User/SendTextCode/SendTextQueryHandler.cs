using HyperMarket.Data;
using HyperMarket.DomainObjects;
using HyperMarket.Queries.ApiManagers;
using HyperMarket.Queries.ApiManagers.Models;
using HyperMarket.Queries.User.SendTextCode.Validation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.User.SendTextCode
{
    public class SendTextQueryHandler : BusinessLogicQueryHandler<SendTextQuery, SendTextResult>
    {
        private readonly RepositoryContextBase Context;
        private readonly ITextMessageManager TextMessageManager;

        public SendTextQueryHandler(RepositoryContextBase context, ITextMessageManager textMessageManager)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
            TextMessageManager = textMessageManager ?? throw ErrorHelper.ArgNull(nameof(textMessageManager));
        }

        public override async Task<SendTextResult> Handle(SendTextQuery input)
        {
            var validator = new SendTextQueryValidation();
            validator.ValidateObject(input);
            var repo = Context.GetRepository<PhoneConfirmation>();
            var remaining = repo
                .Where(x => x.Phone == input.Phone)
                .ToList();
            remaining.ForEach(x => x.Disabled = true);
            repo.SaveChanges();

            var code = new Random().Next(1, 100000).ToString();
            code = code.PadLeft(5, '0');
            await TextMessageManager.SendMessage(new TextMessageModel { 
                Code = code,
                PhoneNumber = input.Phone
            });

            return new SendTextResult();
        }
    }
}
