using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries
{
    public static class ImageConstants
    {
        /// <summary>
        /// Expects {0} as Store id
        /// </summary>
        public const string StoreOriginalFormat = "store/{0}/original";

        /// <summary>
        /// Expects {0} as Store id, {1} size
        /// </summary>
        public const string StoreResizedFormat = "store/{0}/{1}";

        /// <summary>
        /// Expects {0} as product id
        /// </summary>
        public const string ProductOriginalFormat = "product/{0}/original";

        /// <summary>
        /// Expects {0} as product id, {1} size
        /// </summary>
        public const string ProductResizedFormat = "product/{0}/{1}";

        /// <summary>
        /// Expects {0} as product id, {1} product stock id
        /// </summary>
        public const string ProductStockOriginalFormat = "product/{0}/stock/{1}/original";

        /// <summary>
        /// Expects {0} as product Id, {1} product stock id, {2} size
        /// </summary>
        public const string ProductStockResizedFormat = "product/{0}/stock/{1}/{2}";
    }
}
