using Scala.StockSimulation.Web.Areas.Admin.Services;
using Xunit;

namespace Scala.StockSimulation.Web.Areas.Admin.Tests;

public class ShippingCostsStrategyTests{
    public class ShippingCostStrategyTests
    {
        [Fact]
        public void CalculateDiscount_BelowThreshold_ReturnsNegativeShippingCost()
        {
            // Arrange
            var strategy = new ShippingCostStrategy(500, 50); 
            var price = 100M; 
            var quantity = 3; 
            var expectedNegativeDiscount = -50M;

            // Act
            var discount = strategy.CalculateDiscount(price, quantity);

            // Assert
            Assert.Equal(expectedNegativeDiscount, discount); 
        }

        [Fact]
        public void CalculateDiscount_AboveThreshold_ReturnsZeroCost()
        {
            // Arrange
            var strategy = new ShippingCostStrategy(500, 50); 
            var price = 200M; 
            var quantity = 3; 
            var expectedNotCost = 0M;

            // Act
            var discount = strategy.CalculateDiscount(price, quantity);

            // Assert
            Assert.Equal(expectedNotCost, discount); 
        }
    }

}