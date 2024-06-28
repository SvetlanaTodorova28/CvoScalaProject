using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.StockSimulation.Core.Entities
{
	public class UserProductState : BaseEntity
	{
		public string Name { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
		public Guid ApplicationUserId { get; set; }
		public int PhysicalStock { get; set; }
		public int FictionalStock { get; set; }
		public int MinimumStock { get; set; }
		public int MaximumStock { get; set; }
		public int SoonAvailableStock { get; set; }
		public int ReservedStock { get; set; }
		public Product Product { get; set; }
		public Guid ProductId { get; set; }
		public Order Order { get; set; }
		public Guid? OrderId { get; set; }
		public string TransactionType { get; set; }
		public int Quantity { get; set; }
	}
}
