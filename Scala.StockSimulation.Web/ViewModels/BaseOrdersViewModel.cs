using System.Reflection.Metadata.Ecma335;

namespace Scala.StockSimulation.Web.ViewModels
{
    public class BaseOrdersViewModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int PhysicalStock { get; set; }
        public int FictionalStock { get; set; }
        public int SoonAvailableStock { get; set; }
        public int ReservedStock { get; set; }
        public int MinimumStock { get; set; }
        public int MaximumStock { get; set; }
        public int Quantity { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }
        public string OrderType { get; set; }
        public Guid OrderId { get; set; }

        public string CustomerName { get; set; }
    }
}
