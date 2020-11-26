using HyperMarket.Data;
using HyperMarket.Queries.Store.AddLogo.Validation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using HMStore = HyperMarket.DomainObjects.Store;
using System.Threading.Tasks;
using System.Linq;
using HyperMarket.BlobServices;

namespace HyperMarket.Queries.Store.AddLogo
{
    public class AddLogoQueryHandler : BusinessLogicQueryHandler<AddLogoQuery, AddLogoResult>
    {
        private readonly RepositoryContextBase Context;
        private readonly IBlobManager BlobManager;

        public AddLogoQueryHandler(RepositoryContextBase context, IBlobManager blobManager)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
            BlobManager = blobManager ?? throw ErrorHelper.ArgNull(nameof(blobManager));
        }

        public override async Task<AddLogoResult> Handle(AddLogoQuery input)
        {
            var validator = new AddLogoQueryValidator();
            validator.ValidateObject(input);

            var repo = Context.GetRepository<HMStore>();
            var storeExists = repo.Where(x => x.StoreId == input.StoreId).Any();

            if (!storeExists)
            {
                throw ErrorHelper.NotFound("No such store");
            }

            var originalKey = string.Format(ImageConstants.StoreOriginalFormat, input.StoreId);
            await BlobManager.UploadAsync(originalKey, input.Image);
            var url = await BlobManager.GetDownloadUrl(originalKey);
            return new AddLogoResult
            {
                Url = url
            };
        }
    }
}
