using HyperMarket.Data;
using HyperMarket.DomainObjects;
using HyperMarket.Queries.Store.Validation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMStore = HyperMarket.DomainObjects.Store;

namespace HyperMarket.Queries.Store.Create
{
    public class CreateStoreQueryHandler : BusinessLogicQueryHandler<CreateStoreQuery, CreateStoreResult>
    {
        private readonly RepositoryContextBase Context;

        public CreateStoreQueryHandler(RepositoryContextBase context)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
        }

        public override Task<CreateStoreResult> Handle(CreateStoreQuery input)
        {
            var validation = new StoreQueryValidation();
            validation.ValidateObject(input);

            var repo = Context.GetRepository<HMStore>();
            var exists = repo.Where(x => x.Name == input.Name).Any();
            var addressRepo = Context.GetRepository<Address>();

            if (exists)
            {
                throw ErrorHelper.Duplicate("Name already exists");
            }

            if (input.AddressId.HasValue)
            {
                var addressExist = addressRepo.Where(x => x.AddressId == input.AddressId.Value).Any();
                if (!addressExist)
                {
                    throw ErrorHelper.NotFound($"Address with id {input.AddressId} not found");
                }
            }

            var store = new HMStore
            {
                Name = input.Name,
                DateCreated = DateTime.UtcNow,
                DisplayName = input.DisplayName,
                Description = input.Description
            };

            if (input.AddressId.HasValue)
            {
                store.AddressId = input.AddressId.Value;
            }
            else if (
                !string.IsNullOrWhiteSpace(input.StreetAddress) ||
                !string.IsNullOrWhiteSpace(input.House) ||
                !string.IsNullOrWhiteSpace(input.Appartment) ||
                !string.IsNullOrWhiteSpace(input.Entrance) ||
                !string.IsNullOrWhiteSpace(input.Floor) ||
                !string.IsNullOrWhiteSpace(input.City) ||
                !string.IsNullOrWhiteSpace(input.Region) ||
                !string.IsNullOrWhiteSpace(input.PostalCode))
            {
                store.Address = new Address
                {
                    StreetAddress = input.StreetAddress,
                    House = input.House,
                    Appartment = input.Appartment,
                    Entrance = input.Entrance,
                    Floor = input.Floor,
                    City = input.City,
                    Region = input.Region,
                    PostalCode = input.PostalCode
                };
            }

            repo.Store(store);
            repo.SaveChanges();

            var result = new CreateStoreResult { 
                StoreId = store.StoreId,
                Name = store.Name,
                AddressId = store.AddressId
            };

            return Task.FromResult(result);
        }
    }
}
