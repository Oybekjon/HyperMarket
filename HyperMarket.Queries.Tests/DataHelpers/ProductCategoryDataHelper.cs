using HyperMarket.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;
using HMProductCategory = HyperMarket.DomainObjects.ProductCategory;

namespace HyperMarket.Queries.Tests.DataHelpers
{
    public static class ProductCategoryDataHelper
    {
        public static ProductCategoryFluentDataHelper AddProductCategory(
            this MainContext context,
            string name,
            string nameUz,
            string nameRu,
            string nameTj,
            long? parentCategoryId,
            out long productCategoryId,
            Action<HMProductCategory> setupAction = null)
        {
            var product = new HMProductCategory
            {
                Name = name,
                NameRu = nameRu,
                NameTj = nameTj,
                NameUz = nameUz,
                ParentProductCategoryId = parentCategoryId
            };
            setupAction?.Invoke(product);
            context.ProductCategories.Add(product);
            context.SaveChanges();
            productCategoryId = product.ProductCategoryId;
            return new ProductCategoryFluentDataHelper(context, productCategoryId);
        }

        public static ProductCategoryFluentDataHelper AddProductCategory(
            this MainContext context,
            string name,
            long? parentCategoryId,
            out long productCategoryId,
            Action<HMProductCategory> setupAction = null)
        {
            return context.AddProductCategory(name, name, name, name, parentCategoryId, out productCategoryId, setupAction);
        }

        public static ProductCategoryFluentDataHelper AddProductCategory(
            this MainContext context,
            string name,
            out long productCategoryId,
            Action<HMProductCategory> setupAction = null)
        {
            return context.AddProductCategory(name, null, out productCategoryId, setupAction);
        }

        public static ProductCategoryFluentDataHelper AddProductCategory(
            this MainContext context,
            string name,
            Action<HMProductCategory> setupAction = null)
        {
            return context.AddProductCategory(name, null, out _, setupAction);
        }

        public class ProductCategoryFluentDataHelper
        {
            public MainContext Context { get; }
            public long ParentId { get; }

            public ProductCategoryFluentDataHelper(MainContext context, long parentId)
            {
                Context = context;
                ParentId = parentId;
            }

            public ProductCategoryFluentDataHelper AddChild(string name)
            {
                Context.AddProductCategory(name, ParentId, out _);
                return this;
            }

            public ProductCategoryFluentDataHelper AddChild(string name, out long id)
            {
                Context.AddProductCategory(name, ParentId, out id);
                return this;
            }

            public ProductCategoryFluentDataHelper AddChildAndSwitch(string name)
            {
                return Context.AddProductCategory(name, ParentId, out _);
            }

            public ProductCategoryFluentDataHelper AddChildAndSwitch(string name, out long id)
            {
                return Context.AddProductCategory(name, ParentId, out id);
            }
        }
    }
}
