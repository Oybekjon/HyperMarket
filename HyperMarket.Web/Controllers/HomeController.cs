using System;
using HyperMarket.Web.ActionHelpers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HyperMarket.Web.Models;
using HyperMarket.Queries;
using HyperMarket.Queries.User.Login;

namespace HyperMarket.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QueryManager Manager;
        public HomeController(ILogger<HomeController> logger, QueryManager manager)
        {
            _logger = logger;
            Manager = manager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {
            var queryHandler = Manager.Resolve<LoginQuery, LoginResult>();
            var vm = await queryHandler.Handle(new LoginQuery());
            return Json(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
