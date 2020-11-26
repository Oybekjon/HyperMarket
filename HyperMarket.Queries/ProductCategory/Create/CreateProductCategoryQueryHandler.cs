using HyperMarket.Data;
using HyperMarket.Queries.ProductCategory.Validation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMProductCategory = HyperMarket.DomainObjects.ProductCategory;

namespace HyperMarket.Queries.ProductCategory.Create
{
    public class CreateProductCategoryQueryHandler : BusinessLogicQueryHandler<CreateProductCategoryQuery, CreateProductCategoryResult>
    {
        private readonly RepositoryContextBase Context;
        public CreateProductCategoryQueryHandler(RepositoryContextBase context)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
        }

        public override Task<CreateProductCategoryResult> Handle(CreateProductCategoryQuery input)
        {
            var validator = new ProductCategoryQueryValidator(true);
            validator.ValidateObject(input);
            var obj = new HMProductCategory
            {
                Name = input.ProductCategory.Name,
                NameRu = input.ProductCategory.NameRu,
                NameUz = input.ProductCategory.NameUz,
                NameTj = input.ProductCategory.NameTj,
                ParentProductCategoryId = input.ProductCategory.ParentProductCategoryId
            };

            var repo = Context.GetRepository<HMProductCategory>();
            if (input.ProductCategory.ParentProductCategoryId.HasValue)
            {
                var parentExists = repo
                    .Where(x => x.ProductCategoryId == input.ProductCategory.ParentProductCategoryId)
                    .Any();

                if (!parentExists)
                {
                    throw ErrorHelper.NotFound("Parent category not found");
                }
            }

            repo.Store(obj);
            repo.SaveChanges();
            input.ProductCategory.ProductCategoryId = obj.ProductCategoryId;
            return Task.FromResult(new CreateProductCategoryResult
            {
                ProductCategory = input.ProductCategory
            });
        }
    }
}
