namespace Scala.StockSimulation.Web.Areas.Admin.Services;

public class ShippingCostStrategy : IDiscountStrategy
{
    private decimal thresholdForFreeShippingCost;
    private decimal shippingCost;

    public ShippingCostStrategy(decimal threshold, decimal standardShippingCost)
    {
        thresholdForFreeShippingCost = threshold;
        shippingCost = standardShippingCost;
    }

    public decimal CalculateDiscount(decimal price, int quantity)
    {
        
        if ((quantity * price) >= thresholdForFreeShippingCost && thresholdForFreeShippingCost!= 0){
            return 0 ;
        }
        return -shippingCost;
    }
}