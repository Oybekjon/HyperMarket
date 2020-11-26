using HyperMarket.Data;
using HyperMarket.DomainObjects;
using HyperMarket.Queries.Errors;
using HyperMarket.Queries.Store.Validation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HMStore = HyperMarket.DomainObjects.Store;

namespace HyperMarket.Queries.Store.Update
{
    public class UpdateStoreQueryHandler : BusinessLogicQueryHandler<UpdateStoreQuery, UpdateStoreResult>
    {
        private readonly RepositoryContextBase Context;

        public UpdateStoreQueryHandler(RepositoryContextBase context)
        {
            Context = context ?? throw ErrorHelper.ArgNull(nameof(context));
        }

        public override Task<UpdateStoreResult> Handle(UpdateStoreQuery input)
        {
            var validator = new StoreQueryValidation();
            validator.ValidateObject(input);
            if (!input.StoreId.HasValue)
            {
                throw new ParameterInvalidException("StoreId must be supplied");
            }

            var repo = Context.GetRepository<HMStore>();
            var stores = repo
                .Include(x => x.Address)
                .Where(x => x.StoreId == input.StoreId || x.Name == input.Name)
                .ToList();

            var target = stores.FirstOrDefault(x => x.StoreId == input.StoreId);

            if (target == null)
            {
                throw ErrorHelper.NotFound("No such store");
            }

            var existingSameName = stores.FirstOrDefault(x => input.Name.Equals(x.Name)); ;
            if (existingSameName != null)
            {
                throw ErrorHelper.Duplicate("Duplicate name");
            }

            target.Name = input.Name;
            target.Description = input.Description;
            target.DisplayName = input.DisplayName;

            if (AddressNeedsChange(target.Address, input))
            {
                if (input.AddressId.HasValue)
                {
                    target.AddressId = input.AddressId;
                }
                else if (HasAddressValues(input))
                {
                    target.Address = new Address();

                    target.Address.StreetAddress = input.StreetAddress;
                    target.Address.House = input.House;
                    target.Address.Appartment = input.Appartment;
                    target.Address.Entrance = input.Entrance;
                    target.Address.Floor = input.Floor;
                    target.Address.City = input.City;
                    target.Address.Region = input.Region;
                    target.Address.PostalCode = input.PostalCode;
                }
            }

            repo.SaveChanges();

            return Task.FromResult(new UpdateStoreResult());
        }

        private bool AddressNeedsChange(Address existingAddress, UpdateStoreQuery query)
        {
            if (HasAddressValues(query) && query.AddressId.HasValue)
            {
                throw ErrorHelper.InvalidOperation("You cannot pass values and address Id");
            }

            if (HasAddressValues(query) && existingAddress == null)
            {
                return true;
            }
            else if (HasAddressValues(query) && existingAddress != null)
            {
                return
                    query.StreetAddress != existingAddress.StreetAddress ||
                    query.House != existingAddress.House ||
                    query.Appartment != existingAddress.Appartment ||
                    query.Entrance != existingAddress.Entrance ||
                    query.Floor != existingAddress.Floor ||
                    query.City != existingAddress.City ||
                    query.Region != existingAddress.Region ||
                    query.PostalCode != existingAddress.PostalCode;
            }

            if (query.AddressId.HasValue)
            {
                return true;
            }

            return false;

        }
        private bool HasAddressValues(UpdateStoreQuery query)
        {
            return
                !string.IsNullOrWhiteSpace(query.StreetAddress) ||
                !string.IsNullOrWhiteSpace(query.House) ||
                !string.IsNullOrWhiteSpace(query.Appartment) ||
                !string.IsNullOrWhiteSpace(query.Entrance) ||
                !string.IsNullOrWhiteSpace(query.Floor) ||
                !string.IsNullOrWhiteSpace(query.City) ||
                !string.IsNullOrWhiteSpace(query.Region) ||
                !string.IsNullOrWhiteSpace(query.PostalCode);
        }
    }
}
