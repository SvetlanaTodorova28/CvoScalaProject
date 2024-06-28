using Scala.StockSimulation.Web.ViewModels;

namespace Scala.StockSimulation.Web.Areas.Admin.ViewModels
{
    public class AdminBaseOrderViewModel : BaseOverviewViewModel
    {
        public Guid OrderId { get; set; }
        public DateTime Date { get; set; }
        public string OrderType { get; set; }
        public string Status { get; set; }
    }
}
