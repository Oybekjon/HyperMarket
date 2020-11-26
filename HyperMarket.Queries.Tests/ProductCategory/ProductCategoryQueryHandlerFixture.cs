using HyperMarket.Queries.ProductCategory;
using HyperMarket.Queries.Tests.DataHelpers;
using HyperMarket.Queries.ViewModels.ProductCategories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests.ProductCategory
{
    public class ProductCategoryQueryHandlerFixture<TQuery, TResult> : BaseDataFixture<TQuery, TResult>
        where TQuery : ProductCategoryQuery, IQuery<TResult>, new()
    {
        public string TopCategory { get; }
        public long TopCategoryId { get; private set; }
        public string SecondLevel1 { get; }
        public long SecondLevel1Id { get; private set; }
        public string ThirdLevel1_1 { get; }
        public long ThirdLevel1_1Id { get; private set; }
        public string ThirdLevel1_2 { get; }
        public long ThirdLevel1_2Id { get; private set; }
        public string SecondLevel2 { get; }
        public long SecondLevel2Id { get; private set; }
        public string ThirdLevel2_1 { get; }
        public long ThirdLevel2_1Id { get; private set; }
        public string ThirdLevel2_2 { get; }
        public long ThirdLevel2_2Id { get; private set; }

        public string NewCatName { get; }
        public long NotExistingCategoryId { get; }

        public ProductCategoryQueryHandlerFixture()
        {
            TopCategory = "TopCategory";
            SecondLevel1 = "SecondLevel1";
            ThirdLevel1_1 = "ThirdLevel1_1";
            ThirdLevel1_2 = "ThirdLevel1_2";
            SecondLevel2 = "SecondLevel2";
            ThirdLevel2_1 = "ThirdLevel2_1";
            ThirdLevel2_2 = "ThirdLevel2_2";
            NewCatName = "NewCatName";

            NotExistingCategoryId = uint.MaxValue;
        }

        public TQuery GetQuery(
            string name = null,
            long? categoryId = null,
            long? parentCategoryId = null
        )
        {
            return new TQuery
            {
                ProductCategory = new ProductCategoryViewModel
                {
                    Name = name ?? NewCatName,
                    NameRu = name ?? NewCatName,
                    NameUz = name ?? NewCatName,
                    NameTj = name ?? NewCatName,
                    ProductCategoryId = categoryId,
                    ParentProductCategoryId = parentCategoryId
                }
            };
        }

        protected override void InitDatabase()
        {
            var top = Context.AddProductCategory(TopCategory, out var topCategoryId);
            TopCategoryId = topCategoryId;
            var ch1 = top.AddChildAndSwitch(SecondLevel1, out var ch1Id);
            var ch2 = top.AddChildAndSwitch(SecondLevel2, out var ch2Id);

            SecondLevel1Id = ch1Id;
            SecondLevel2Id = ch2Id;

            ch1.AddChild(ThirdLevel1_1, out var thirdLevel1_1Id)
               .AddChild(ThirdLevel1_2, out var thirdLevel1_2Id);
            ch2.AddChild(ThirdLevel2_1, out var thirdLevel2_1Id)
               .AddChild(ThirdLevel2_2, out var thirdLevel2_2Id);

            ThirdLevel1_1Id = thirdLevel1_1Id;
            ThirdLevel1_2Id = thirdLevel1_2Id;
            ThirdLevel2_1Id = thirdLevel2_1Id;
            ThirdLevel2_2Id = thirdLevel2_2Id;
        }
    }
}
