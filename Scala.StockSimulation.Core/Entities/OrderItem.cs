using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.StockSimulation.Core.Entities
{
	public class OrderItem: BaseEntity
	{
		public Product Product { get; set; }
		public Guid ProductId { get; set; }
		public Order Order { get; set; }
		public Guid OrderId { get; set; }
		public int Quantity { get; set; }
	}
}
