using Xunit;

namespace Scala.StockSimulation.Web.Areas.Admin.Tests;

public class VolumeDiscountStrategyTests{
   
        [Fact]
        public void CalculateDiscount_AboveThreshold_ReturnsCorrectDiscount()
        {
            // Arrange
            var strategy = new VolumeDiscountStrategy(10, 10); 
            var price = 100M; 
            var quantity = 10; 
            var expectedDiscount = 10M;

            // Act
            var discount = strategy.CalculateDiscount(price, quantity);

            // Assert
            Assert.Equal(expectedDiscount, discount); 
        }

        [Fact]
        public void CalculateDiscount_BelowThreshold_ReturnsZeroDiscount()
        {
            // Arrange
            var strategy = new VolumeDiscountStrategy(10, 10); 
            var price = 100M; 
            var quantity = 5; 
            var expectedDiscount = 0M;

            // Act
            var discount = strategy.CalculateDiscount(price, quantity);

            // Assert
            Assert.Equal(expectedDiscount, discount); 
        }
    }

