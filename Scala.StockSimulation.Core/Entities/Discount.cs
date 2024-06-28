using Scala.StockSimulation.Core.Enums;

namespace Scala.StockSimulation.Core.Entities;

public class Discount{
    public Guid Id { get; set; }
    public DiscountType Type { get; set; }
    public bool IsActive { get; set; }
    public decimal? Rate { get; set; }
    
    public ICollection<Product> Products { get; set; }

}