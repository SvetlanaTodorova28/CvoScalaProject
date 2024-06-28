using Microsoft.AspNetCore.Mvc;

namespace Scala.StockSimulation.Web.ViewModels
{
    public class BaseProductViewModel
    {
        public string Text { get; set; }
        [HiddenInput]
        public Guid ProductId { get; set; }
        public string ArticleNumber { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public BaseViewModel Supplier { get; set; }
    }
}
