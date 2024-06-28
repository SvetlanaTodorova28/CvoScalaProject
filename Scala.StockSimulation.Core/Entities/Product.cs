using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.StockSimulation.Core.Entities
{
	public class Product : BaseEntity
	{	
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public decimal PriceWithDiscounts { get; set; }
		public string ArticleNumber { get; set; }
		public Supplier Supplier { get; set; }
		public Guid SupplierId { get; set; }
		public int InitialStock { get; set; }
		public int InitialMaximumStock { get; set; }
		public int InitialMinimumStock { get; set; }
		public ICollection<Discount> Discounts { get; set; }
	}
}
