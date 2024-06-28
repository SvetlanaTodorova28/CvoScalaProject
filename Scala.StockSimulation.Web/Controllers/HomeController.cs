using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scala.StockSimulation.Web.Models;
using SmartBreadcrumbs.Attributes;
using System.Diagnostics;

namespace Scala.StockSimulation.Web.Controllers
{


    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Teacher"))
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin" });
                }

                if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Index", "Overview");
                }
            }
            return View();
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