using Scala.StockSimulation.Web.Areas.Admin.Services;

public class VolumeDiscountStrategy : IDiscountStrategy
{
    private decimal discountRate;
    private int thresholdQuantity;

    public VolumeDiscountStrategy(decimal rate, int threshold)
    {
        discountRate = rate;
        thresholdQuantity = threshold;
    }

    public decimal CalculateDiscount(decimal price, int quantity)
    {
        if (quantity >= thresholdQuantity)
        {
            return price * discountRate / 100;
        }
        return 0;
    }
}