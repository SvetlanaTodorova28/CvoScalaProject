using Scala.StockSimulation.Core.Entities;

namespace Scala.StockSimulation.Web.ViewModels
{
	public class OverviewShowAllOrdersViewModel
	{
		public IEnumerable<BaseOrderViewModel> Orders { get; set; }
	}
}
