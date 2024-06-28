using Scala.StockSimulation.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Scala.StockSimulation.Web.ViewModels
{
    public class OrdersPlaceOrderViewModel
    {
        public List<OrdersSelectedProductsSupplierViewModel> Products { get; set; }
        [MaxLength(100)]
        [Display(Name = "Klantnaam")]
        public string CustomerName { get; set; }
        public string OrderType { get; set; }
    }
}
