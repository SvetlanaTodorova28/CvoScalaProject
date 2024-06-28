
using Scala.StockSimulation.Web.Areas.Admin.ViewModels.Discounts;

namespace Scala.StockSimulation.Web.Areas.Admin.Services;

public interface IFormBuilder{
    public List<DiscountViewModel> GetDiscountTypes();
}