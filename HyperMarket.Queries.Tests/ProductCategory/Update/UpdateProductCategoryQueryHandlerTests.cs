using HyperMarket.Errors;
using HyperMarket.Queries.Errors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Tests.ProductCategory.Update
{
    [TestClass]
    public class UpdateProductCategoryQueryHandlerTests
    {
        [TestMethod]
        public async Task Update_Ok()
        {
            using var fixture = new UpdateProductCategoryQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(categoryId: fixture.TopCategoryId);

            var result = await service.Handle(query);

            Assert.IsNotNull(result);
            var entity = fixture.Context.ProductCategories.FirstOrDefault(x => x.ProductCategoryId == fixture.TopCategoryId);
            Assert.IsNotNull(entity);
            Assert.AreEqual(fixture.NewCatName, entity.Name);
            Assert.AreEqual(fixture.NewCatName, entity.NameUz);
            Assert.AreEqual(fixture.NewCatName, entity.NameRu);
            Assert.AreEqual(fixture.NewCatName, entity.NameTj);
        }

        [TestMethod]
        public async Task Update_NotFoundCategory_Fail()
        {
            using var fixture = new UpdateProductCategoryQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(categoryId: fixture.NotExistingCategoryId);

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task Update_EmptyName_Fail()
        {
            using var fixture = new UpdateProductCategoryQueryHandlerFixture();
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
