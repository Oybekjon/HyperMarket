using HyperMarket.Queries.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.User.Hashing
{
    public class HashingQueryHandler : BusinessLogicQueryHandler<HashingQuery, HashingResult>
    {
        private readonly EncryptionSettings Settings;
        public HashingQueryHandler(EncryptionSettings settings)
        {
            Guard.NotNull(settings, nameof(settings));
            Guard.PropertyNotNullOrEmpty(settings.HashingKey, nameof(settings.HashingKey));
            Settings = settings;
        }

        public override Task<HashingResult> Handle(HashingQuery input)
        {
            Guard.NotNull(input, nameof(input));
            Guard.NotNullOrEmpty(input.PlainText, nameof(input.PlainText));
            var helper = new EncryptionHelper();
            var result = helper.EncryptedHash(input.PlainText, Settings.HashingKey);
            return Task.FromResult(new HashingResult
            {
                HashedText = result
            });
        }
    }
}
