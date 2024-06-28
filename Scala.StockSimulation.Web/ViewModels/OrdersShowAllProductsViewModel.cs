using Microsoft.AspNetCore.Mvc;

namespace Scala.StockSimulation.Web.ViewModels
{
    public class OrdersShowAllProductsViewModel
    {
        public List<BaseProductViewModel> Products { get; set; }
    }
}
