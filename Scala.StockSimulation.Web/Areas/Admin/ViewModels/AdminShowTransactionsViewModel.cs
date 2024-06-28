namespace Scala.StockSimulation.Web.Areas.Admin.ViewModels
{
    public class AdminShowTransactionsViewModel
    {
        public IEnumerable<AdminBaseOrderViewModel> Transactions { get; set; }
        public string StudentName { get; set; }
    }
}
