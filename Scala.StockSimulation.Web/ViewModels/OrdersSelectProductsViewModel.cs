using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Scala.StockSimulation.Web.ViewModels
{
    public class OrdersSelectProductsViewModel
    {
        public List<CheckboxViewModel> Products { get; set; }
        [HiddenInput]
        public Guid SupplierId { get; set; }
        [HiddenInput]
        public List<BaseViewModel> SelectedProducts { get; set; }
    }
}
