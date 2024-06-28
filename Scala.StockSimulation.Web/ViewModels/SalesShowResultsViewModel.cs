
namespace Scala.StockSimulation.Web.ViewModels
{
    public class SalesShowResultsViewModel : SalesIndexViewModel
    {
        public List<BaseOrdersViewModel> SalesData { get; set; }
        public string ChartType { get; set; }
        public string StudentName { get; set; }
        public bool IsStudent { get; set; }
    }
}
