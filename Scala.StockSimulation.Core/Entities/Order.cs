using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.StockSimulation.Core.Entities
{
    public class Order : BaseEntity
    {
        public ApplicationUser ApplicationUser { get; set; }
        public Guid ApplicationUserId { get; set; }
        public OrderType OrderType { get; set; }
        public Guid OrderTypeId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DateDelivered { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<UserProductState> UserProductStates { get; set; }
        public long OrderNumber { get; set; }
    }
}
