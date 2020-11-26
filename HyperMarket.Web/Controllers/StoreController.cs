using HyperMarket.Queries.Store.GetList;
using HyperMarket.Web.ActionHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyperMarket.Web.Controllers
{
    public class StoreController : Controller
    {
        private readonly ILogger<StoreController> Logger;
        private readonly QueryManager Manager;

        public StoreController(ILogger<StoreController> logger, QueryManager manager)
        {
            Logger = logger;
            Manager = manager;
        }

        [HttpGet]
        public async Task<StoreListResult> GetStores(StoreListQuery query)
        {
            var handler = Manager.Resolve<StoreListQuery, StoreListResult>();
            var result = await handler.Handle(query);
            return result;
        }
    }
}
