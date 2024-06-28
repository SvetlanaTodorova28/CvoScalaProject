using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scala.StockSimulation.Core.Entities;
using Scala.StockSimulation.Core.Enums;
using Scala.StockSimulation.Utilities;
using Scala.StockSimulation.Web.Areas.Admin.Services;
using Scala.StockSimulation.Web.Areas.Admin.ViewModels;
using Scala.StockSimulation.Web.Areas.Admin.ViewModels.Discounts;
using Scala.StockSimulation.Web.Data;
using SmartBreadcrumbs.Attributes;

namespace Scala.StockSimulation.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "TeacherPolicy")]
public class ProductsController : Controller{

    private readonly ScalaStockSimulationDbContext _scalaStockSimulationDbContext;
    private readonly IFormBuilder _formBuilder;
    

    public ProductsController(ScalaStockSimulationDbContext scalaStockSimulationDbContext, IFormBuilder formBuilder){
        _scalaStockSimulationDbContext = scalaStockSimulationDbContext;
        _formBuilder = formBuilder;
    }

    [HttpGet]
    [Breadcrumb("Producten", AreaName = "Admin", FromAction = "Index", FromController = typeof(AdminController))]
    public ActionResult Index(){
        var products = _scalaStockSimulationDbContext
            .Products
            .Include(p => p.Supplier)
            .ToList();
        var productsIndexViewModel = new ProductsIndexViewModel();
        productsIndexViewModel.Products = products
            .Select(p => new ProductsInfoViewModel{
                Id = p.Id,
                Name = p.Name,
                SupplierName = p.Supplier.Name
            }).ToList();
        ViewBag.Title = "Producten";
        return View(productsIndexViewModel);
    }

    [HttpGet]
    [Breadcrumb("Toon info product", AreaName = "Admin", FromAction = "Index", FromController = typeof(ProductsController))]
    public IActionResult Info(Guid id){
        var product = _scalaStockSimulationDbContext
            .Products
            .Include(p => p.Supplier)
            .Include(p => p.Discounts)
            .FirstOrDefault(p => p.Id == id);

        ProductsInfoViewModel productsInfoViewModel = new ProductsInfoViewModel(){
            Id = Guid.Parse(id.ToString()),
            Name = product.Name,
            SupplierName = product.Supplier.Name,
            Price = product.Price,
            PriceWithDiscounts = product.PriceWithDiscounts,
        };
        productsInfoViewModel.Discounts = product.Discounts
            .Select(discount => new DiscountViewModel{
                Type = discount.Type.ToText(),
                Rate = discount.Rate,
            })
            .ToList();

       ViewBag.Title = "Info product";
        return View(productsInfoViewModel);
    }
    
    [HttpGet]
    [Breadcrumb("Bewerk prijs", AreaName = "Admin", FromAction = "Index", FromController = typeof(ProductsController))]
    public ActionResult Edit(Guid id){
        if (HttpContext.Session.Keys.Contains("newPrice")){
            HttpContext.Session.Remove("newPrice");
        }
        if (HttpContext.Session.Keys.Contains("productId")){
            id = Guid.Parse(HttpContext.Session.GetString("productId"));
            HttpContext.Session.Remove("productId");
        }
        var product = _scalaStockSimulationDbContext
            .Products
            .Include(p => p.Discounts)
            .Include(p => p.Supplier)
            .FirstOrDefault(p => p.Id == id);
        var productEditViewModel = new ProductsEditViewModel{
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            SupplierName = product.Supplier.Name,
            ExamplePriceWithDiscounts = product.PriceWithDiscounts,
            Discounts = _formBuilder.GetDiscountTypes(),
        };
        var availableDiscounts = product.Discounts.Select(d => d.Type.ToText());
        foreach (var checkbox in productEditViewModel.Discounts){
            if (availableDiscounts.Contains(checkbox.Type)){
                checkbox.IsActive = true;
            }
        }
        if (product.Discounts.Where(d => d.Type == DiscountType.Volume)!= null){
            productEditViewModel.VolumeDiscountRate =
                product.Discounts.Where(d => d.Type == DiscountType.Volume).Select(d =>d.Rate).FirstOrDefault();
        }
        if (product.Discounts.Where(d => d.Type == DiscountType.Supplier)!= null){
            productEditViewModel.SupplierDiscountRate =
                product.Discounts.Where(d => d.Type == DiscountType.Supplier).Select(d => d.Rate).FirstOrDefault();
        }
        if (product.Discounts.Where(d => d.Type == DiscountType.Shipping)!= null){
            productEditViewModel.ShippingCost =
                product.Discounts.Where(d => d.Type == DiscountType.Shipping).Select(d => d.Rate).FirstOrDefault();
        }
        ViewBag.Title = "Bewerk prijs";
        return View(productEditViewModel);
    }

    [HttpPost]
    [Breadcrumb("Bewerk prijs", AreaName = "Admin", FromAction = "Edit", FromController = typeof(ProductsController))]
    public ActionResult Edit(ProductsEditViewModel productsEditViewModel){
       
        if (!ModelState.IsValid){
            productsEditViewModel.Discounts = _formBuilder.GetDiscountTypes();
            return View(productsEditViewModel);
        }
        if (HttpContext.Session.Keys.Contains("newPrice")){
            HttpContext.Session.Remove("newPrice");
        }
        var productToUpdate = _scalaStockSimulationDbContext.Products
            .Include(p => p.Discounts)
            .FirstOrDefault(p => p.Id == productsEditViewModel.Id);

        if (productToUpdate != null){
            productToUpdate.Price = productsEditViewModel.Price;

            var discountService = new DiscountService();
            if (productToUpdate.Discounts.Count > 0){
                foreach (var discount in productToUpdate.Discounts){
                    _scalaStockSimulationDbContext.Discounts.Remove(discount);
                }
                productToUpdate.Discounts.Clear();
                productToUpdate.PriceWithDiscounts = productsEditViewModel.Price;
                _scalaStockSimulationDbContext.SaveChanges();
            }
            
        foreach (var discountViewModel in productsEditViewModel.Discounts){
            if (discountViewModel.IsActive){
                ApplyDiscount(productsEditViewModel, discountViewModel, productToUpdate, discountService);
            }
            
        }
        productToUpdate.PriceWithDiscounts = discountService.ApplyDiscounts(productToUpdate.Price, productsEditViewModel.Quantity ?? 0);
            _scalaStockSimulationDbContext.SaveChanges();
        }
        HttpContext.Session.SetString("newPrice", "");
        return RedirectToAction("Index", "Products", new{ area = "Admin" });
    }
    

    [HttpPost]
    [Breadcrumb("Toon bewerkte prijs", AreaName = "Admin", FromAction = "Edit", FromController = typeof(ProductsController))]
    public IActionResult CalculateNewPrice(ProductsEditViewModel productsEditViewModel){
        if (!ModelState.IsValid){
            HttpContext.Session.SetString("productId", productsEditViewModel.Id.ToString());
            return View("Edit", productsEditViewModel);
        }

        var productToUpdate = _scalaStockSimulationDbContext.Products
            .Include(p => p.Discounts)
            .FirstOrDefault(p => p.Id == productsEditViewModel.Id);
        if (productToUpdate == null){
            return NotFound();
        }
        
        var discountService = new DiscountService();
        foreach (var discountViewModel in productsEditViewModel.Discounts){
            if (discountViewModel.IsActive){
                ApplyDiscount(productsEditViewModel, discountViewModel, productToUpdate, discountService);
            }
        }

        productsEditViewModel.ExamplePriceWithDiscounts = discountService.ApplyDiscounts(productsEditViewModel.Price, productsEditViewModel.Quantity?? 0);
        productsEditViewModel.Name = productToUpdate.Name;
        productsEditViewModel.Price = productsEditViewModel.ExamplePriceWithDiscounts?? 0;
        
        HttpContext.Session.SetString("newPrice", productsEditViewModel.Price.ToString("0.00"));
        HttpContext.Session.SetString("productId", productToUpdate.Id.ToString());
        ViewBag.Title = "Toon bewerkte prijs";
        return View("Edit",productsEditViewModel);
    }

    private void ApplyDiscount(ProductsEditViewModel productsEditViewModel,DiscountViewModel discountViewModel, Product productToUpdate, DiscountService discountService)
    {
        if (!discountViewModel.IsActive)
        {
            return;
        }

        switch (discountViewModel.Type){
            case GlobalConstants.Supplier:
                discountService.AddDiscountStrategy(new SupplierDiscountStrategy(productsEditViewModel.SupplierDiscountRate ?? 0));
                var discountSupplier = new Discount{
                    Type = (DiscountType)Enum.Parse(typeof(DiscountType), DiscountType.Supplier.ToString()),
                    Rate = productsEditViewModel.SupplierDiscountRate
                };
                productToUpdate.Discounts.Add(discountSupplier);
                break;
            case GlobalConstants.Shipping:
                discountService.AddDiscountStrategy(new ShippingCostStrategy(productsEditViewModel.FreeShippingThreshold ?? 0,productsEditViewModel.ShippingCost?? 0),true);
                var discountShipping = new Discount{
                    Type = (DiscountType)Enum.Parse(typeof(DiscountType), DiscountType.Shipping.ToString()),
                    Rate = productsEditViewModel.ShippingCost,
                };
                productToUpdate.Discounts.Add(discountShipping);
                break;
            case GlobalConstants.Volume:
                discountService.AddDiscountStrategy(new VolumeDiscountStrategy(productsEditViewModel.VolumeDiscountRate?? 0,productsEditViewModel.QuantityForVolumeDiscount?? 0));
                var discountVolume = new Discount{
                    Type = (DiscountType)Enum.Parse(typeof(DiscountType), DiscountType.Volume.ToString()),
                    Rate = productsEditViewModel.VolumeDiscountRate
                };
                productToUpdate.Discounts.Add(discountVolume);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}