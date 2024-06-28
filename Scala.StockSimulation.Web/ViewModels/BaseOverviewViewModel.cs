namespace Scala.StockSimulation.Web.ViewModels
{
    public class BaseOverviewViewModel
    {
        public IEnumerable<BaseOrdersViewModel> UserProductStates { get; set; }
        public string OrderNr{ get; set; }
        public string CustomerName { get; set; }
    }
}
