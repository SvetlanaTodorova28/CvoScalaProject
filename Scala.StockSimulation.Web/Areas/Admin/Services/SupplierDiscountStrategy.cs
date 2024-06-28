using Scala.StockSimulation.Core.Entities;

namespace Scala.StockSimulation.Web.Areas.Admin.Services;


public class SupplierDiscountStrategy : IDiscountStrategy
{
    private decimal discountRate;

    public SupplierDiscountStrategy(decimal rate)
    {
        discountRate = rate;
    }

    public decimal CalculateDiscount(decimal price, int quantity)
    {
        return price * discountRate / 100;
    }
}





