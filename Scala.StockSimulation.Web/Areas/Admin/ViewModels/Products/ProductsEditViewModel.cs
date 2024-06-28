using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

using Scala.StockSimulation.Web.Areas.Admin.ViewModels.Discounts;

namespace Scala.StockSimulation.Web.Areas.Admin.ViewModels;

public class ProductsEditViewModel{
    [HiddenInput]
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    [Display(Name = "Prijs")]
    [RegularExpression(@"^\d+,\d{0,2}$", ErrorMessage = "Je moet een komma en tot twee decimalen gebruiken voor de prijs.")]
    public decimal Price { get; set; }
    
    [Display(Name = "Leverancier")]
    public string SupplierName { get; set; }
    
    [Display(Name = "Drempelwaarde voor gratis verzending")]
    [Range(0, int.MaxValue, ErrorMessage = "Voer een geldig geheel getal in voor de drempelwaarde van gratis verzending.")]
    public int? FreeShippingThreshold { get; set; }
    
    [Display(Name = "Nodige aantal voor staffelkorting")]
    [Range(0, int.MaxValue, ErrorMessage = "Voer een geldig geheel getal")]
    public int? QuantityForVolumeDiscount { get; set; }
    
    [Display(Name = "Prijs met kortingen")]
    public decimal? ExamplePriceWithDiscounts { get; set; }
    
    public List<DiscountViewModel> Discounts { get; set; }
    
    [Display(Name = "Aantal")]
    [Range(1, int.MaxValue, ErrorMessage = "Het aantal moet een geheel getal zijn.")]
    public int? Quantity { get; set; }
    
    [Display(Name = "Percentage voor staffelkortingen")]
    [RegularExpression(@"^\d+,\d{0,2}$", ErrorMessage = "Je moet een komma en tot twee decimalen gebruiken voor het percentage.")] 
    [Range(1, 100, ErrorMessage = "Het percentage moet tussen 1 en 100 liggen.")]
    public decimal? VolumeDiscountRate { get; set; }
    
    [Display(Name = "Percentage voor leverancierskorting")]
    [RegularExpression(@"^\d+,\d{0,2}$", ErrorMessage = "Je moet een komma en tot twee decimalen gebruiken voor het percentage.")] 
    [Range(1, 100, ErrorMessage = "Het percentage moet tussen 1 en 100 liggen.")]
    public decimal? SupplierDiscountRate { get; set; }
    
    [Display(Name = "Transportkosten")]
    [RegularExpression(@"^\d+,\d{0,2}$", ErrorMessage = "Je moet een komma en tot twee decimalen gebruiken voor de transportkosten.")]
    [Range(1, Double.MaxValue, ErrorMessage = "Er moet een positief getal ingevoerd worden.")]
    public decimal? ShippingCost { get; set; }
   
  
}