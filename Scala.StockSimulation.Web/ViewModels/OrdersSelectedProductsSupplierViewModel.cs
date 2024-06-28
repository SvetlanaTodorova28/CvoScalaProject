using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Scala.StockSimulation.Web.ViewModels
{
    public class OrdersSelectedProductsSupplierViewModel
    {
        [HiddenInput]
        public Guid ProductId { get; set; }
        [HiddenInput]
        public string ProductName { get; set; }
        [HiddenInput]
        public int PhysicalStock { get; set; }
        [HiddenInput]
        public int FictionalStock { get; set; }
        [HiddenInput]
        public int SoonAvailableStock { get; set; }
        [HiddenInput]
        public int ReservedStock { get; set; }
        [HiddenInput]
        public int MinimumStock { get; set; }
        [HiddenInput]
        public int MaximumStock { get; set; }
        [Required(ErrorMessage = "Aantal is verplicht.")]
        [Range(1, int.MaxValue, ErrorMessage = $"Gelieve een positief getal te kiezen")]
        public int? Quantity { get; set; }
    }
}
