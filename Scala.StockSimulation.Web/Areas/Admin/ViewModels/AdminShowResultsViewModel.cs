using Scala.StockSimulation.Web.ViewModels;

namespace Scala.StockSimulation.Web.Areas.Admin.ViewModels
{
	public class AdminShowResultsViewModel : BaseOverviewViewModel
	{
		public string Name { get; set; }
		public Guid ApplicationUserId { get; set; }
		public string SearchTerm { get; set; }
		public IEnumerable<BaseViewModel> Orders{ get; set; }
	}
}
