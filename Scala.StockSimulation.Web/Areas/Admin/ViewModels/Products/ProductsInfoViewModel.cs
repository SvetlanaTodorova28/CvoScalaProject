using Scala.StockSimulation.Web.Areas.Admin.ViewModels.Discounts;

namespace Scala.StockSimulation.Web.Areas.Admin.ViewModels;

public class ProductsInfoViewModel{
    public Guid Id {get; set;}
    public string Name {get; set;}
    public string SupplierName {get; set;}
    public decimal Price {get; set;}
    public decimal PriceWithDiscounts {get; set;}
    
    public decimal ShippingCosts {get; set;}
    public List<DiscountViewModel> Discounts { get; set; }
}