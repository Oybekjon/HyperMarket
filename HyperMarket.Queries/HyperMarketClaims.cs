using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HyperMarket.Queries
{
    public static class HyperMarketClaims
    {
        public const string Admin = "Admin";
        public const string DeliveryPerson = "DeliveryPerson";
        public const string ProductManager = "ProductManager";
        public const string StoreUserManager = "StoreUserManager";
        public const string StoreProductManager = "StoreProductManager";
        public const string StoreDetailsManager = "StoreDetailsManager";

        private readonly static IReadOnlyCollection<string> _claims;

        static HyperMarketClaims()
        {
            _claims = typeof(HyperMarketClaims)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(x => x.IsLiteral && !x.IsInitOnly)
                .Select(x => x.GetValue(null))
                .Cast<string>()
                .ToArray();
        }

        public static IReadOnlyCollection<string> AllClaims()
        {
            return _claims;
        }

        public static IReadOnlyCollection<string> StoreClaims()
        {
            return _claims.Where(x => x.StartsWith("Store")).ToArray();
        }

        public static IReadOnlyCollection<string> NonStoreClaims()
        {
            return _claims.Where(x => !x.StartsWith("Store")).ToArray();
        }
    }
}
