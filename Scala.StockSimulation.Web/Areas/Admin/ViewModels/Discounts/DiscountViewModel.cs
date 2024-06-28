using Microsoft.AspNetCore.Mvc;

namespace Scala.StockSimulation.Web.Areas.Admin.ViewModels.Discounts;

public class DiscountViewModel{
    [HiddenInput]
    public int Id { get; set; }
    [HiddenInput]
    public string Type { get; set; }
    [HiddenInput]
    public decimal? Rate { get; set; }
    public bool IsActive { get; set; }
    
   public decimal FreeShippingThreshold{get; set; }
}