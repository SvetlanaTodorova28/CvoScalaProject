namespace Scala.StockSimulation.Web.ViewModels
{
    public class SalesSelectProductsViewModel : SalesIndexViewModel
    {
        public List<CheckboxViewModel> Products { get; set; }
        public string ChartType { get; set; }
    }
}
