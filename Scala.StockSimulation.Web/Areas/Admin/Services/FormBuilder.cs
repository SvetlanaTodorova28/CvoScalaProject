using Scala.StockSimulation.Core.Enums;

using Scala.StockSimulation.Web.Areas.Admin.ViewModels.Discounts;

namespace Scala.StockSimulation.Web.Areas.Admin.Services;

public class FormBuilder:IFormBuilder{
    public List<DiscountViewModel> GetDiscountTypes(){

        var discountTypes = Enum.GetValues(typeof(DiscountType))
            .Cast<DiscountType>()
            .Select(item => item.ToText());
        return discountTypes.Select(item => new DiscountViewModel(){
            Type = item,
            Id = item.Length
        }).ToList();
    }
}