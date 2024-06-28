using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.StockSimulation.Core.Entities
{
	public class OrderType : BaseEntity
	{
		public string Name { get; set; }
		public ICollection<Order> Orders { get; set; }
	}
}
