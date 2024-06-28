using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Scala.StockSimulation.Core.Entities;
using Scala.StockSimulation.Utilities.Authorization;
using Scala.StockSimulation.Web.Data;
using Scala.StockSimulation.Web.ViewModels;
using SmartBreadcrumbs.Attributes;

namespace Scala.StockSimulation.Web.Controllers
{
    public class OverviewController : Controller
    {
        private readonly ScalaStockSimulationDbContext _scalaStockSimulationDbContext;
        private readonly ILogger<OverviewController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public OverviewController(ScalaStockSimulationDbContext scalaStockSimulationDbContext, ILogger<OverviewController> logger, UserManager<ApplicationUser> userManager)
        {
            _scalaStockSimulationDbContext = scalaStockSimulationDbContext;
            _logger = logger;
            _userManager = userManager;
        }

        [AuthorizeMultiplePolicy("StudentPolicy,TeacherPolicy", false)]
        [HttpGet]
        [Breadcrumb("Overzicht")]
        public async Task<IActionResult> Index()
        {
            if (User.Claims.FirstOrDefault(c => c.Value == "Teacher") != null)
                return RedirectToAction("ShowStudents", "Admin", new { area = "Admin" });

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var userProductStatesFromUser = _scalaStockSimulationDbContext.UserProductStates.Where(u => u.ApplicationUserId == currentUser.Id).ToList();

            OverviewIndexViewModel overviewIndexViewModel = new OverviewIndexViewModel()
            {
                HasUserProductStates = userProductStatesFromUser.Count() == 0
            };

            return View(overviewIndexViewModel);
        }

        [Authorize(Policy = "StudentPolicy")]
        public async Task<IActionResult> InitStartSituation()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var products = _scalaStockSimulationDbContext.Products.ToList();
            var userProductStates = products.Select(p => new UserProductState
            {
                Name = p.Name,
                ApplicationUserId = currentUser.Id,
                PhysicalStock = p.InitialStock,
                FictionalStock = p.InitialStock,
                MinimumStock = p.InitialMinimumStock,
                MaximumStock = p.InitialMaximumStock,
                SoonAvailableStock = 0,
                ReservedStock = 0,
                ProductId = p.Id,
                TransactionType = "Start",
                Quantity = 0
            }).ToList();

            foreach (var userProductState in userProductStates)
            {
                _scalaStockSimulationDbContext.UserProductStates.Add(userProductState);
            }

            try
            {
                _scalaStockSimulationDbContext.SaveChanges();
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException.Message);
                ModelState.AddModelError("", "Kan de veranderingen niet opslaan" +
                                            "Gelieve opnieuw te proberen");
                return RedirectToAction("Index", "Overview");
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "StudentPolicy")]
        [Breadcrumb("Overzicht Bestellingen", FromAction = "Index", FromController = typeof(OverviewController))]
        public async Task<IActionResult> ShowAllOrders()
        {
            OverviewShowAllOrdersViewModel showAllOrdersViewModel = new OverviewShowAllOrdersViewModel();

            var orders = await _scalaStockSimulationDbContext.Orders.Where(o => o.ApplicationUser.UserName == User.Identity.Name)
                .Include(o => o.OrderType)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Supplier)
                .ToListAsync();

            showAllOrdersViewModel.Orders = orders.Select(o => new BaseOrderViewModel
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber.ToString(),
                Value = o.OrderType.Name,
                DateOrdered = o.Created,
                OrderQuantity = o.OrderItems.Count(),
                SupplierName = orders
                   .Where(x => x.Id == o.Id)
                   .Select(x => x.OrderItems)
                   .SelectMany(x => x)
                   .Select(x => x.Product)
                   .Select(x => x.Supplier)
                   .FirstOrDefault().Name,
                Status = o.DateDelivered == null ? "Niet Geleverd" : "Geleverd",
                OrderTypeId = o.OrderType.Id,
                CustomerName = o.CustomerName == null ? string.Empty : o.CustomerName
            }).OrderBy(o => o.OrderNumber);
            return View(showAllOrdersViewModel);
        }
        
        [Authorize(Policy = "StudentPolicy")]
        [HttpGet]
        [Breadcrumb("Informatie bestelling", FromAction = "ShowAllOrders", FromController = typeof(OverviewController))]
        public IActionResult OrderInfo(Guid orderId)
        {

            var order = _scalaStockSimulationDbContext
                .Orders
                .Include(o => o.UserProductStates)
                .ThenInclude(u => u.Product)
                .FirstOrDefault(o => o.Id == orderId);
            string customerName = string.Empty;
            if (order.CustomerName != null)
            {
                customerName = order.CustomerName;
            }

            var overviewOrderInfoViewModel = new OverviewOrderInfoViewModel
            {
                UserProductStates = order.UserProductStates.Select(op => new BaseOrdersViewModel
                {
                    ProductId = op.ProductId,
                    ProductName = op.Product.Name,
                    PhysicalStock = op.PhysicalStock,
                    FictionalStock = op.FictionalStock,
                    SoonAvailableStock = op.SoonAvailableStock,
                    ReservedStock = op.ReservedStock,
                    MinimumStock = op.MinimumStock,
                    MaximumStock = op.MaximumStock,
                    Quantity = op.Quantity,
                    Status = op.Updated == null ? "Besteld" : "Geleverd",
                    Created = op.Created,
                    OrderType = op.TransactionType
                }).OrderBy(u => u.ProductId),
                OrderNr = order.OrderNumber.ToString(),
                CustomerName = customerName
            };

            return View(overviewOrderInfoViewModel);
        }
    }
}








