using Scala.StockSimulation.Web.Areas.Admin.Services;
using Xunit;

namespace Scala.StockSimulation.Web.Areas.Admin.Tests;

public class DiscountServiceTests{
   
  
    [Theory]
    [InlineData(5,80,451,140 )]
    [InlineData(5,6,1000,140)]
    public void ApplyDiscounts_WithShippingCostAndSupplierDiscountWithoutVolumeDiscount_DiscountedPrice(
        int quantity, 
        int volumeDiscountThreshold,
        decimal  shippingCostThreshold,
        decimal expectedFinalPrice)
    {
        // Arrange
        var service = new DiscountService();
        service.AddDiscountStrategy(new SupplierDiscountStrategy(10)); 
        service.AddDiscountStrategy(new VolumeDiscountStrategy(5, volumeDiscountThreshold)); 
        service.AddDiscountStrategy(new ShippingCostStrategy(shippingCostThreshold, 50), isShipping: true); 

        var originalPrice = 100M; 
        var shippingCost = 50M; 

        // Act
        var finalPrice = service.ApplyDiscounts(originalPrice, quantity);

        // Assert
        Assert.Equal(expectedFinalPrice, finalPrice);
    }
    [Theory]
    [InlineData(10, 10,15,5, 100,81)]
    [InlineData(10, 10,15,14, 140,81)]
    [InlineData(1, 1,2,2, 196,98.01)]
    public void ApplyDiscounts_WithMultipleDiscountsNoShippingCosts_DiscountedPrice(
        decimal supplierDiscountRate, 
        decimal volumeDiscountRate, 
        int quantity,
        int volumeDiscountThreshold, 
        decimal shippingCostThreshold,
        decimal expectedFinalPrice)
    {
        // Arrange
        var service = new DiscountService();
        service.AddDiscountStrategy(new SupplierDiscountStrategy(supplierDiscountRate)); 
        service.AddDiscountStrategy(new VolumeDiscountStrategy(volumeDiscountRate, volumeDiscountThreshold)); 
        service.AddDiscountStrategy(new ShippingCostStrategy(shippingCostThreshold, 50), isShipping: true); 

        var originalPrice = 100M; 

        // Act
        var finalPrice = service.ApplyDiscounts(originalPrice, quantity);
        
        // Assert
        Assert.Equal(expectedFinalPrice, finalPrice);
    }


    }

