using Scala.StockSimulation.Web.Data;

namespace Scala.StockSimulation.Web.Services
{
    public class OrderService
    {
        private readonly ScalaStockSimulationDbContext _scalaStockSimulationDbContext;

        public OrderService(ScalaStockSimulationDbContext scalaStockSimulationDbContext)
        {
            _scalaStockSimulationDbContext = scalaStockSimulationDbContext;
        }

        public long GetOrderNumber()
        {
            var lastOrder = _scalaStockSimulationDbContext.Orders.OrderByDescending(x => x.Created).FirstOrDefault();

            if (lastOrder == null)
            {
				return 1;
			}

            return lastOrder.OrderNumber + 1;
		}
	}
}
