using Scala.StockSimulation.Web.Areas.Admin.Services;
using Xunit;

public class SupplierDiscountStrategyTests
{
    [Fact]
    public void CalculateDiscount_GivenPositiveRate_ReturnsCorrectDiscount()
    {
        // Arrange
        var strategy = new SupplierDiscountStrategy(10); 
        var price = 100M; 

        // Act
        var discount = strategy.CalculateDiscount(price, 5); 

        // Assert
        Assert.Equal(10M, discount); 
    }
    
}