using Scala.StockSimulation.Utilities;

namespace Scala.StockSimulation.Core.Enums;

public enum DiscountType{
  
        Supplier,
        Volume,
        Shipping,
       
        
}
public static class DiscountTypeExtensions
{
        public static string ToText(this DiscountType discountType){

                return discountType switch{
                        DiscountType.Supplier => GlobalConstants.Supplier,
                        DiscountType.Volume => GlobalConstants.Volume,
                        DiscountType.Shipping => GlobalConstants.Shipping,
                      
                        _ => "Unknown discount type",
                };

        }
}