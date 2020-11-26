using HyperMarket.Errors;
using HyperMarket.Queries.Errors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Tests.ProductCategory.Create
{
    [TestClass]
    public class CreateProductCategoryQueryHandlerTests
    {
        [TestMethod]
        public async Task Create_Ok()
        {
            using var fixture = new CreateProductCategoryQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery();

            var result = await service.Handle(query);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ProductCategory);
            Assert.IsNotNull(result.ProductCategory.ProductCategoryId);
            var entity = fixture.Context.ProductCategories
                .FirstOrDefault(x => x.ProductCategoryId == result.ProductCategory.ProductCategoryId);
            Assert.IsNotNull(entity);
            Assert.AreEqual(fixture.NewCatName, entity.Name);
            Assert.AreEqual(fixture.NewCatName, entity.NameUz);
            Assert.AreEqual(fixture.NewCatName, entity.NameRu);
            Assert.AreEqual(fixture.NewCatName, entity.NameTj);
        }

        [TestMethod]
        public async Task Create_NotFoundCategory_Fail()
        {
            using var fixture = new CreateProductCategoryQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(parentCategoryId: fixture.NotExistingCategoryId);

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task Create_EmptyName_Fail()
        {
            using var fixture = new CreateProductCategoryQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(categoryId: fixture.TopCategoryId);
            query.ProductCategory.Name = "";
            query.ProductCategory.NameUz = "";
            query.ProductCategory.NameRu = "";
            query.ProductCategory.NameTj = "";

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => service.Handle(query));
        }
    }
}
