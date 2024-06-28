using Scala.StockSimulation.Core.Entities;

namespace Scala.StockSimulation.Web.Areas.Admin.Services;

public interface IDiscountStrategy{
    decimal CalculateDiscount(decimal price, int quantity);
}