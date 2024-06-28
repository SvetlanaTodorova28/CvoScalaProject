using Scala.StockSimulation.Core.Entities;

namespace Scala.StockSimulation.Web.ViewModels
{
	public class BaseOrderViewModel : BaseViewModel
	{
		public DateTime DateOrdered { get; set; }
		public DateTime? DateDelivered { get; set; }
		public int OrderQuantity { get; set; }
		
		public string SupplierName { get; set; }
		public string Status { get; set; }
		public Guid OrderTypeId { get; set; }
        public string CustomerName { get; set; }
        public string OrderNumber { get; set; }
    }
}
