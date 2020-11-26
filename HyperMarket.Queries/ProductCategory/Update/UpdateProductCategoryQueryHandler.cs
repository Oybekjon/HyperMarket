using HyperMarket.Data;
using HyperMarket.Queries.ProductCategory.Validation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HMProductCategory = HyperMarket.DomainObjects.ProductCategory;

namespace HyperMarket.Queries.ProductCategory.Update
{
    public class UpdateProductCategoryQueryHandler : BusinessLogicQueryHandler<UpdateProductCategoryQuery, UpdateProductCategoryResult>
    {
        private readonly RepositoryContextBase Context;
        public UpdateProductCategoryQueryHandler(RepositoryContextBase context)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
        }

        public override Task<UpdateProductCategoryResult> Handle(UpdateProductCategoryQuery input)
        {
            var validator = new ProductCategoryQueryValidator(false);
            validator.ValidateObject(input);

            var repo = Context.GetRepository<HMProductCategory>();
            var result = repo.FirstOrDefault(x => x.ProductCategoryId == input.ProductCategory.ProductCategoryId);
            if (result == null)
            {
                throw ErrorHelper.NotFound("No such product category");
            }

            result.Name = input.ProductCategory.Name;
            result.NameRu = input.ProductCategory.NameRu;
            result.NameUz = input.ProductCategory.NameUz;
            result.NameTj = input.ProductCategory.NameTj;
            result.ParentProductCategoryId = input.ProductCategory.ParentProductCategoryId;
            repo.SaveChanges();

            return Task.FromResult(new UpdateProductCategoryResult());
        }
    }
}
