using HyperMarket.Data;
using HyperMarket.Queries.Validation;
using HyperMarket.Queries.ViewModels.Store;
using System.Linq;
using System.Threading.Tasks;
using HMStore = HyperMarket.DomainObjects.Store;

namespace HyperMarket.Queries.Store.GetList
{
    public class StoreListQueryHandler : BusinessLogicQueryHandler<StoreListQuery, StoreListResult>
    {
        private readonly RepositoryContextBase Context;
        public StoreListQueryHandler(RepositoryContextBase context)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
        }

        public override Task<StoreListResult> Handle(StoreListQuery input)
        {
            var validator = new ListQueryValidator<StoreListQuery, StoreListResult>();
            validator.ValidateObject(input);

            var repo = Context.GetRepository<HMStore>();
            var query = (IQueryable<HMStore>)null;
            var totalRecords = 0;
            if (!input.StoreId.HasValue)
                (query, totalRecords) = GetQuery(input, repo);
            else
                query = repo.Where(x => x.StoreId == input.StoreId);

            var materialized = query.Select(x => new StoreViewModel
            {
                Name = x.Name,
                StoreId = x.StoreId,
                Description = x.Description,
                DisplayName = x.DisplayName
            }).ToList();

            var viewModel = new StoreListResult(materialized, totalRecords);
            return Task.FromResult(viewModel);
        }

        private static (IQueryable<HMStore>, int) GetQuery(StoreListQuery input, IRepository<HMStore> repo)
        {
            var query = repo.GetAll()
                            .OrderBy(x => x.Name)
                            .AsQueryable();

            var totalRecords = query.Count();
            if (input.Offset.HasValue)
            {
                query = query.Skip(input.Offset.Value);
            }

            var limit = input.Limit ?? ushort.MaxValue;
            query = query.Take(limit);

            return (query, totalRecords);
        }
    }
}
