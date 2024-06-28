using Scala.StockSimulation.Core.Entities;

namespace Scala.StockSimulation.Web.Areas.Admin.Services;

public class DiscountService{
   
        private readonly List<IDiscountStrategy> strategies = new List<IDiscountStrategy>();
        private IDiscountStrategy shippingStrategy;

        public decimal ApplyDiscounts(decimal originalPrice, int quantity)
        {
            decimal currentPrice = originalPrice;
            foreach (var strategy in strategies)
            {
                currentPrice -= strategy.CalculateDiscount(currentPrice, quantity);
            }
            if (shippingStrategy != null)
                currentPrice -= shippingStrategy.CalculateDiscount(currentPrice, quantity);

            return currentPrice;
        }


        public void AddDiscountStrategy(IDiscountStrategy strategy,bool isShipping = false)
        {
            
            if (isShipping)
                shippingStrategy = strategy;
            else
                strategies.Add(strategy);
        }
    }

