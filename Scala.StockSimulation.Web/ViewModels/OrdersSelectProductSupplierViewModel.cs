using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Scala.StockSimulation.Web.ViewModels
{
    public class OrdersSelectProductSupplierViewModel
    {       
        public IEnumerable<SelectListItem> Suppliers { get; set; }
        [Display(Name = "Kies leverancier")]
        public Guid SupplierId { get; set; }
    }
}
