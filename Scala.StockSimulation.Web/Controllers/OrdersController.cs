
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Scala.StockSimulation.Core.Entities;
using Scala.StockSimulation.Web.Data;
using Scala.StockSimulation.Web.Services;
using Scala.StockSimulation.Web.ViewModels;
using SmartBreadcrumbs.Attributes;


namespace Scala.StockSimulation.Web.Controllers
{
    [Authorize(Policy = "StudentPolicy")]
    public class OrdersController : Controller
    {
        private readonly ScalaStockSimulationDbContext _scalaStockSimulationDbContext;
        private readonly OrderService _orderService;

        public OrdersController(ScalaStockSimulationDbContext scalaStockSimulationDbContext)
        {
            _scalaStockSimulationDbContext = scalaStockSimulationDbContext;
            _orderService = new OrderService(scalaStockSimulationDbContext);
        }

        public IActionResult Index()
        {
            return View();
        }

        [Breadcrumb("Plaats bestelling", FromAction = "SelectProducts", FromController = typeof(OrdersController))]
        [HttpGet]
        public IActionResult PlaceOrder(IEnumerable<Guid> selectedProductsIds)
        {
            if (!HttpContext.Session.Keys.Contains("ordertype"))
            {
                return NotFound();
            }
            var ordertype = HttpContext.Session.GetString("ordertype");
            var ordertypeName = "";
            if (ordertype == "00000000-0000-0000-0000-000000000016")
            {
                ordertypeName = "leverancier";
            }
            else
            {
                ordertypeName = "klant";
            }
            var username = User.Identity.Name;

            ApplicationUser currentUser = _scalaStockSimulationDbContext.ApplicationUsers
                .Include(u => u.UserProductState)
                .FirstOrDefault(c => c.UserName.Equals(username));
            var selectedProducts = _scalaStockSimulationDbContext.Products
                .Include(p => p.Supplier)
                .Where(p => selectedProductsIds.Contains(p.Id)).ToList();
            var userProductStates = currentUser.UserProductState.ToList();

            var selectedUserProductStates = selectedProducts.Select(p =>
            {
                var productUserProductStates = userProductStates.Where(u => u.ProductId == p.Id);
                return productUserProductStates.OrderByDescending(ups => ups.Created).FirstOrDefault();
            });

            var ordersPlaceOrderViewModel = new OrdersPlaceOrderViewModel();

            if (TempData.ContainsKey("SerializedOrder"))
            {
                var serializedOrder = (string)TempData["SerializedOrder"];
                ordersPlaceOrderViewModel = JsonConvert.DeserializeObject<OrdersPlaceOrderViewModel>(serializedOrder);
            }
            else
            {
                ordersPlaceOrderViewModel = new OrdersPlaceOrderViewModel
                {
                    Products = selectedUserProductStates.Select(u => new OrdersSelectedProductsSupplierViewModel
                    {
                        ProductId = u.Product.Id,
                        ProductName = u.Name,
                        PhysicalStock = u.PhysicalStock,
                        FictionalStock = u.FictionalStock,
                        SoonAvailableStock = u.SoonAvailableStock,
                        ReservedStock = u.ReservedStock,
                        MinimumStock = u.MinimumStock,
                        MaximumStock = u.MaximumStock,
                    }).ToList(),
                    OrderType = ordertypeName
                };
            }

            return View(ordersPlaceOrderViewModel);
        }


        [Breadcrumb("Kies leverancier", FromAction = "SelectOrderType", FromController = typeof(OrdersController))]
        [HttpGet]
        public IActionResult SelectProductSupplier(Guid ordertypeId)
        {
            HttpContext.Session.SetString("ordertype", ordertypeId.ToString());

            var allSuppliers = _scalaStockSimulationDbContext
                .Suppliers
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList();
            allSuppliers.Insert(0, new SelectListItem { Text = "Alle leveranciers", Value = "00000000-0000-0000-0000-000000000099" });

            var ordersSelectProductSupplierViewModel = new OrdersSelectProductSupplierViewModel
            {
                Suppliers = allSuppliers,
            };

            return View(ordersSelectProductSupplierViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Kies leverancier", FromAction = "SelectOrderType", FromController = typeof(OrdersController))]
        public IActionResult SelectProductSupplier(OrdersSelectProductSupplierViewModel ordersSelectProductSupplierViewModel)
        {
            ordersSelectProductSupplierViewModel
                .Suppliers = _scalaStockSimulationDbContext
                .Suppliers
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name,
                });
            if (!ModelState.IsValid)
            {
                return View(ordersSelectProductSupplierViewModel);
            }
            if (_scalaStockSimulationDbContext
                .Suppliers
                .Any(s => s.Id == ordersSelectProductSupplierViewModel.SupplierId))
            {
                return RedirectToAction(nameof(SelectProducts), new { supplierId = ordersSelectProductSupplierViewModel.SupplierId });
            }
            if (ordersSelectProductSupplierViewModel.SupplierId == Guid.Parse("00000000-0000-0000-0000-000000000099"))
            {
                return RedirectToAction(nameof(ShowAllProducts));
            }
            return View(nameof(Index));
        }

        [HttpGet]
        [Breadcrumb("Kies Producten", FromAction = "SelectProductSupplier", FromController = typeof(OrdersController))]
        public IActionResult SelectProducts(Guid supplierId){
            Guid sessionSupplierId;
            if (HttpContext.Session.Keys.Contains("supplierId") &&
                Guid.TryParse(HttpContext.Session.GetString("supplierId"), out sessionSupplierId)){
                if (sessionSupplierId != supplierId){
                    HttpContext.Session.SetString("supplierId", supplierId.ToString());
                }
            }
            else{
                HttpContext.Session.SetString("supplierId", supplierId.ToString());
            }
            var supplier = _scalaStockSimulationDbContext
                .Suppliers
                .FirstOrDefault(s => s.Id == supplierId);
            if (supplier == null)
            {
                if (HttpContext.Session.Keys.Contains("ordertype"))
                {
                    var ordertypeId = Guid.Parse(HttpContext.Session.GetString("ordertype"));
                    return RedirectToAction(nameof(SelectProductSupplier), new { ordertypeId = ordertypeId });
                }
                return RedirectToAction(nameof(SelectOrderType));
            }

            var ordersSelectProductsViewModel = new OrdersSelectProductsViewModel
            {
                Products = _scalaStockSimulationDbContext
                .Products
                .Where(p => p.SupplierId == supplierId)
                .Select(p => new CheckboxViewModel
                {
                    ProductId = p.Id,
                    Text = p.Name,
                    ArticleNumber = p.ArticleNumber,
                    Description = p.Description,
                    Price = p.Price,
                }).ToList(),
                SupplierId = supplierId
            };
            HttpContext.Session.SetString("supplierId", supplierId.ToString());
            return View(ordersSelectProductsViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Kies Producten", FromAction = "SelectProductSupplier", FromController = typeof(OrdersController))]
        public IActionResult SelectProducts(OrdersSelectProductsViewModel ordersSelectProductsViewModel)
        {

            var selectedProductIds = ordersSelectProductsViewModel.Products
            .Where(p => p.IsSelected)
            .Select(p => p.ProductId);

            var selectedProducts = _scalaStockSimulationDbContext
                .Products
                .Where(p => selectedProductIds.Contains(p.Id))
                .ToList();

            var supplierId = ordersSelectProductsViewModel.SupplierId;
            ordersSelectProductsViewModel = new OrdersSelectProductsViewModel
            {
                Products = _scalaStockSimulationDbContext
               .Products
               .Where(p => p.SupplierId == supplierId)
               .Select(p => new CheckboxViewModel
               {
                   ProductId = p.Id,
                   Text = p.Name,
                   ArticleNumber = p.ArticleNumber,
                   Description = p.Description,
                   Price = p.Price,
               }).ToList(),
                SupplierId = supplierId
            };
            if (selectedProducts.Count == 0)
            {
                ModelState.AddModelError("selecteer product", "Gelieve minstens 1 product te selecteren.");
                return View(ordersSelectProductsViewModel);
            }
            if (!ModelState.IsValid)
            {
                return View(ordersSelectProductsViewModel);
            }

            return RedirectToAction(nameof(PlaceOrder), new { selectedProductsIds = selectedProductIds });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Plaats bestelling", FromAction = "SelectProducts", FromController = typeof(OrdersController))]
        public IActionResult PlaceOrder(OrdersPlaceOrderViewModel ordersPlaceOrderViewModel)
        {
            if (!ModelState.IsValid)
            {
                if (!HttpContext.Session.Keys.Contains("ordertype"))
                {
                    return NotFound();
                }
                var ordertype = HttpContext.Session.GetString("ordertype");
                var ordertypeName = "";
                if (ordertype == "00000000-0000-0000-0000-000000000016")
                {
                    ordertypeName = "leverancier";
                }
                else
                {
                    ordertypeName = "klant";
                }
                ordersPlaceOrderViewModel.OrderType = ordertypeName;
                return View(ordersPlaceOrderViewModel);
            }
            if (string.IsNullOrWhiteSpace(ordersPlaceOrderViewModel.CustomerName))
            {
                ordersPlaceOrderViewModel.CustomerName = string.Empty;
            }
            if (ordersPlaceOrderViewModel.Products == null)
            {
                TempData["Message"] = "U hebt geen producten gekozen voor de bestelling, kon de bestelling niet plaatsen. Gelieve het opnieuw te proberen.";
                return RedirectToAction("Index", "Overview");
            }

            bool exceedingProducts = false;

            if (ordersPlaceOrderViewModel.OrderType == "leverancier")
            {
                foreach (var product in ordersPlaceOrderViewModel.Products)
                {
                    if ((product.Quantity + product.FictionalStock) > product.MaximumStock)
                    {
                        exceedingProducts = true;
                    }
                }
            }
            else
            {
                foreach (var product in ordersPlaceOrderViewModel.Products)
                {
                    if ((product.FictionalStock - product.Quantity) < product.MinimumStock)
                    {
                        exceedingProducts = true;
                    }
                }
            }
            if (exceedingProducts)
            {
                var serializedOrder = JsonConvert.SerializeObject(ordersPlaceOrderViewModel);
                TempData["SerializedOrder"] = serializedOrder;
                return View("PlaceOrderConfirmExceedingStock", ordersPlaceOrderViewModel);
            }

            return ProcessOrder(ordersPlaceOrderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmAndSaveOrder()
        {
            var serializedOrder = (string)TempData["SerializedOrder"];
            var ordersPlaceOrderViewModel = JsonConvert.DeserializeObject<OrdersPlaceOrderViewModel>(serializedOrder);
            return ProcessOrder(ordersPlaceOrderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private IActionResult ProcessOrder(OrdersPlaceOrderViewModel ordersPlaceOrderViewModel)
        {
            var user = _scalaStockSimulationDbContext.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);

            Order order = new Order
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = user.Id,
                OrderTypeId = Guid.Parse(HttpContext.Session.GetString("ordertype")),
                Created = DateTime.Now,
                OrderNumber = _orderService.GetOrderNumber(),
                CustomerName = ordersPlaceOrderViewModel.CustomerName == null ? string.Empty : ordersPlaceOrderViewModel.CustomerName
            };

            _scalaStockSimulationDbContext.Orders.Add(order);

            var selectedProducts = ordersPlaceOrderViewModel.Products;

            foreach (var product in selectedProducts)
            {
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = product.ProductId,
                    Quantity = (int)product.Quantity,
                    Created = DateTime.Now,
                };
                _scalaStockSimulationDbContext.OrderItems.Add(orderItem);
            }

            var orderType = _scalaStockSimulationDbContext.OrderTypes.FirstOrDefault(o => o.Id == order.OrderTypeId);

            try
            {
                _scalaStockSimulationDbContext.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("error", "Er is iets fout gegaan");
                return View(ordersPlaceOrderViewModel);
            }
            var newProductStates = ordersPlaceOrderViewModel.Products
                .Where(p => p.Quantity > 0)
                .Select(p => new UserProductState
                {
                    ProductId = p.ProductId,
                    Name = p.ProductName,
                    ApplicationUserId = user.Id,
                    Quantity = p.Quantity.Value,
                    MaximumStock = p.MaximumStock,
                    MinimumStock = p.MinimumStock,
                    PhysicalStock = p.PhysicalStock,
                    ReservedStock = p.ReservedStock,
                    FictionalStock = p.FictionalStock,
                    SoonAvailableStock = p.SoonAvailableStock,
                    Created = DateTime.Now,
                    OrderId = order.Id,
                    TransactionType = orderType.Name
                }).ToList();

            foreach (var userProductState in newProductStates)
            {
                if (orderType.Id == Guid.Parse("00000000-0000-0000-0000-000000000016"))
                {
                    userProductState.SoonAvailableStock = userProductState.SoonAvailableStock + userProductState.Quantity;
                    userProductState.FictionalStock = userProductState.FictionalStock + userProductState.Quantity;
                }
                else
                {
                    userProductState.ReservedStock = userProductState.ReservedStock + userProductState.Quantity;
                    userProductState.FictionalStock = userProductState.FictionalStock - userProductState.Quantity;
                }
            }
            foreach (var newProductState in newProductStates)
            {
                _scalaStockSimulationDbContext.UserProductStates.Add(newProductState);
            }

            try
            {
                _scalaStockSimulationDbContext.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("", "Kan de veranderingen niet opslaan" +
                                                "Gelieve opnieuw te proberen");
                return RedirectToAction(nameof(Index), "Overview");
            }

            return RedirectToAction(nameof(ShowOrderOverview), new { orderid = order.Id });
        }


        public IActionResult ProductsDelivered(Guid orderId)
        {
            var order = _scalaStockSimulationDbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                TempData["Message"] = "Het order werd niet gevonden, gelieve het opnieuw te proberen.";
                return RedirectToAction("Index", "Overview");
            }
            var orderType = _scalaStockSimulationDbContext.OrderTypes.FirstOrDefault(o => o.Id == order.OrderTypeId);

            var productIds = order.OrderItems.Select(oi => oi.ProductId).ToList();

            var userProductStates = productIds.Select(id =>
                _scalaStockSimulationDbContext.UserProductStates
                    .Include(ups => ups.Product)
                    .OrderByDescending(ups => ups.Created)
                    .FirstOrDefault(ups => ups.ProductId == id)).ToList();

            order.DateDelivered = DateTime.Now;
            _scalaStockSimulationDbContext.Orders.Update(order);

            userProductStates.ForEach(o =>
            {
                var quantity = order.OrderItems.FirstOrDefault(oi => oi.ProductId == o.ProductId).Quantity;
                var isSupplier = orderType.Id == Guid.Parse("00000000-0000-0000-0000-000000000016");

                var physicalStock = isSupplier ? o.PhysicalStock + quantity : o.PhysicalStock - quantity;
                var soonAvailableStock = isSupplier ? o.SoonAvailableStock - quantity : o.SoonAvailableStock;
                var reservedStock = isSupplier ? o.ReservedStock : o.ReservedStock - quantity;
                var fictionalStock = physicalStock + (soonAvailableStock - reservedStock);
                _scalaStockSimulationDbContext.UserProductStates.Add(
                    new UserProductState
                    {
                        Id = Guid.NewGuid(),
                        ApplicationUserId = o.ApplicationUserId,

                        Name = o.Product.Name,
                        PhysicalStock = physicalStock,
                        FictionalStock = fictionalStock,
                        MinimumStock = o.MinimumStock,
                        MaximumStock = o.MaximumStock,
                        SoonAvailableStock = soonAvailableStock,
                        ReservedStock = reservedStock,
                        ProductId = o.ProductId,
                        TransactionType = _scalaStockSimulationDbContext.OrderTypes.FirstOrDefault(ot => ot.Id == order.OrderTypeId).Name,
                        Created = DateTime.Now,
                        OrderId = order.Id,
                        Quantity = quantity,
                        Updated = DateTime.Now
                    });
            });
            _scalaStockSimulationDbContext.SaveChanges();

            return RedirectToAction(nameof(ShowProductsDelivered), new { orderId = order.Id });
        }

        [Breadcrumb("Soort bestelling", FromAction = "Index", FromController = typeof(OverviewController))]
        public IActionResult SelectOrderType()
        {
            TempData.Remove("SerializedOrder");
            return View();
        }
        [Breadcrumb("Bestelling Overzicht", FromAction = "SelectProducts", FromController = typeof(OrdersController))]
        [HttpGet]
        public IActionResult ShowOrderOverview(Guid orderId)
        {
            var order = _scalaStockSimulationDbContext.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                TempData["Message"] = "Het order werd niet gevonden, gelieve het opnieuw te proberen.";
                return RedirectToAction("Index", "Overview");
            }
            var ordertypeId = order.OrderTypeId;

            string orderTypeName;
            if (ordertypeId == Guid.Parse("00000000-0000-0000-0000-000000000016"))
            {
                orderTypeName = "bij leverancier";
            }
            else
            {
                orderTypeName = "voor klant";
            }
            var ordersShowOrderOverviewViewModel = new OrdersShowOrderOverviewViewModel
            {
                UserProductStates = _scalaStockSimulationDbContext
                .UserProductStates.Where(u => u.OrderId == orderId)
                .Select(u => new BaseOrdersViewModel
                {
                    ProductId = u.Id,
                    ProductName = u.Name,
                    FictionalStock = u.FictionalStock,
                    MinimumStock = u.MinimumStock,
                    MaximumStock = u.MaximumStock,
                    PhysicalStock = u.PhysicalStock,
                    Quantity = u.Quantity,
                    ReservedStock = u.ReservedStock,
                    SoonAvailableStock = u.SoonAvailableStock,
                    Status = u.Updated == null ? "Besteld" : "Geleverd",
                    Created = u.Created,
                }),
                OrdersTitle = orderTypeName,
                CustomerName = order.CustomerName == null ? string.Empty : order.CustomerName
            };
            return View(ordersShowOrderOverviewViewModel);
        }

        [Breadcrumb("Geleverde Producten", FromAction = "ShowAllOrders", FromController = typeof(OverviewController))]
        public IActionResult ShowProductsDelivered(Guid orderId)
        {
            var order = _scalaStockSimulationDbContext
                .Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                TempData["Message"] = "Het order werd niet gevonden, gelieve het opnieuw te proberen.";
                return RedirectToAction("Index", "Overview");
            }
            var supplierId = order
                .OrderItems
                .Select(oi => oi.Product)
                .Select(p => p.SupplierId).First();
            var supplier = _scalaStockSimulationDbContext
                .Suppliers
                .FirstOrDefault(s => s.Id == supplierId).Name;
            ViewBag.Supplier = supplier;
            string customerName = string.Empty;
            if (order.CustomerName != null)
            {
                customerName = order.CustomerName;
            }

            var orderedProducts = _scalaStockSimulationDbContext
                .UserProductStates
                .Include(ups => ups.Product)
                .Where(ups => ups.OrderId == orderId && ups.Updated != null).ToList();
            OverviewOrderInfoViewModel overviewOrderInfoViewModel = new OverviewOrderInfoViewModel
            {

                UserProductStates = orderedProducts.Select(op => new BaseOrdersViewModel
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

                }).OrderBy(u => u.ProductId),
                OrderNr = order.OrderNumber.ToString(),
                CustomerName = customerName
            };

            return View(overviewOrderInfoViewModel);
        }

        [Breadcrumb("Overzicht producten", FromAction = "SelectProductSupplier", FromController = typeof(OrdersController))]
        public IActionResult ShowAllProducts()
        {
            var ordersShowAllProductsViewModel = new OrdersShowAllProductsViewModel
            {
                Products = _scalaStockSimulationDbContext
                .Products
                .Include(p => p.Supplier)
                .Select(p => new BaseProductViewModel
                {
                    ProductId = p.Id,
                    Text = p.Name,
                    ArticleNumber = p.ArticleNumber,
                    Description = p.Description,
                    Price = p.Price,
                    Supplier = new BaseViewModel
                    {
                        Value = p.Supplier.Name,
                        Id = p.SupplierId
                    }
                }).ToList(),
            };
            return View(ordersShowAllProductsViewModel);
        }
    }
}

